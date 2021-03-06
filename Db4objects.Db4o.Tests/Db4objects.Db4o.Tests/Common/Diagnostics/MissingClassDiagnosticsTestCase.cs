/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2011  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Sharpen.Util;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
    public class MissingClassDiagnosticsTestCase : ITestCase, ITestLifeCycle, IOptOutMultiSession
    {
        private const int Port = unchecked(0xdb40);
        private static readonly string DbUri = "test_db";
        private static readonly string User = "user";
        private static readonly string Password = "password";

        [NonSerialized] private readonly MemoryStorage _storage = new MemoryStorage();

        /// <exception cref="System.Exception"></exception>
        public virtual void SetUp()
        {
            PopulateContainer(Db4oEmbedded.NewConfiguration());
        }

        /// <exception cref="System.Exception"></exception>
        public virtual void TearDown()
        {
        }

        public static void Main(string[] args)
        {
            new ConsoleTestRunner(typeof (MissingClassDiagnosticsTestCase)).Run();
        }

        private void PrepareHost(IFileConfiguration fileConfig, ICommonConfiguration commonConfig
            , IList classesNotFound)
        {
            fileConfig.Storage = _storage;
            PrepareCommon(commonConfig, classesNotFound);
        }

        private void PrepareCommon(ICommonConfiguration commonConfig, IList classesNotFound
            )
        {
            commonConfig.ReflectWith(Platform4.ReflectorForType(typeof (Pilot
                )));
            PrepareDiagnostic(commonConfig, classesNotFound);
        }

        private void PrepareDiagnostic(ICommonConfiguration common, IList classesNotFound
            )
        {
            common.Diagnostic.AddListener(new _IDiagnosticListener_94(classesNotFound));
        }

        public virtual void TestEmbedded()
        {
            IList missingClasses = new ArrayList();
            var excludingConfig = Db4oEmbedded.NewConfiguration();
            PrepareHost(excludingConfig.File, excludingConfig.Common, missingClasses);
            ExcludeClasses(excludingConfig.Common, new[]
            {
                typeof (Pilot
                    ),
                typeof (Car)
            });
            var excludingContainer = Db4oEmbedded.OpenFile(excludingConfig
                , DbUri);
            try
            {
                excludingContainer.Query(new AcceptAllPredicate()
                    );
            }
            finally
            {
                excludingContainer.Close();
            }
            AssertPilotAndCarMissing(missingClasses);
        }

        private void AssertPilotAndCarMissing(IList classesNotFound)
        {
            IList excluded = Arrays.AsList(new[]
            {
                ReflectPlatform.FullyQualifiedName(
                    typeof (Pilot)),
                ReflectPlatform.FullyQualifiedName
                    (typeof (Car))
            });
            Assert.AreEqual(excluded.Count, classesNotFound.Count);
            for (var candidateIter = excluded.GetEnumerator();
                candidateIter.MoveNext
                    ();)
            {
                var candidate = ((string) candidateIter.Current);
                Assert.IsTrue(classesNotFound.Contains(candidate));
            }
        }

        public virtual void TestMissingClassesInServer()
        {
            IList serverMissedClasses = new ArrayList();
            IList clientMissedClasses = new ArrayList();
            var serverConfig = Db4oClientServer.NewServerConfiguration();
            PrepareHost(serverConfig.File, serverConfig.Common, serverMissedClasses);
            ExcludeClasses(serverConfig.Common, new[]
            {
                typeof (Pilot
                    ),
                typeof (Car)
            });
            var server = Db4oClientServer.OpenServer(serverConfig, DbUri, Port);
            server.GrantAccess(User, Password);
            try
            {
                var clientConfig = Db4oClientServer.NewClientConfiguration();
                PrepareCommon(clientConfig.Common, clientMissedClasses);
                var client = Db4oClientServer.OpenClient(clientConfig, "localhost",
                    Port, User, Password);
                client.Query(new AcceptAllPredicate());
                client.Close();
            }
            finally
            {
                server.Close();
            }
            AssertPilotAndCarMissing(serverMissedClasses);
            Assert.AreEqual(0, clientMissedClasses.Count);
        }

        public virtual void TestMissingClassesInClient()
        {
            IList serverMissedClasses = new ArrayList();
            IList clientMissedClasses = new ArrayList();
            var serverConfig = Db4oClientServer.NewServerConfiguration();
            PrepareHost(serverConfig.File, serverConfig.Common, serverMissedClasses);
            var server = Db4oClientServer.OpenServer(serverConfig, DbUri, Port);
            server.GrantAccess(User, Password);
            try
            {
                var clientConfig = Db4oClientServer.NewClientConfiguration();
                PrepareCommon(clientConfig.Common, clientMissedClasses);
                ExcludeClasses(clientConfig.Common, new[]
                {
                    typeof (Pilot
                        ),
                    typeof (Car)
                });
                var client = Db4oClientServer.OpenClient(clientConfig, "localhost",
                    Port, User, Password);
                var result = client.Query(new AcceptAllPredicate
                    ());
                IterateOver(result);
                client.Close();
            }
            finally
            {
                server.Close();
            }
            Assert.AreEqual(0, serverMissedClasses.Count);
            AssertPilotAndCarMissing(clientMissedClasses);
        }

        private void IterateOver(IObjectSet result)
        {
            while (result.HasNext())
            {
                result.Next();
            }
        }

        private void ExcludeClasses(ICommonConfiguration commonConfiguration, Type[] classes
            )
        {
            commonConfiguration.ReflectWith(new ExcludingReflector(ByRef.NewInstance(typeof (Pilot
                )), classes));
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual void TestClassesFound()
        {
            IList missingClasses = new ArrayList();
            var config = Db4oEmbedded.NewConfiguration();
            PrepareHost(config.File, config.Common, missingClasses);
            PopulateContainer(config);
            Assert.AreEqual(0, missingClasses.Count);
        }

        private void PopulateContainer(IEmbeddedConfiguration config)
        {
            config.File.Storage = _storage;
            IObjectContainer container = Db4oEmbedded.OpenFile(config, DbUri);
            try
            {
                var pilot = new Pilot
                    ("Barrichello");
                pilot.GetCars().Add(new Car("BMW"));
                container.Store(pilot);
            }
            finally
            {
                container.Close();
            }
        }

        [Serializable]
        public class AcceptAllPredicate : Predicate
        {
            public virtual bool Match(object candidate)
            {
                return true;
            }
        }

        public class Pilot
        {
            public IList cars = new ArrayList();
            public string name;

            public Pilot(string name)
            {
                this.name = name;
            }

            public virtual IList GetCars()
            {
                return cars;
            }

            public virtual string GetName()
            {
                return name;
            }

            public override string ToString()
            {
                return "Pilot[" + name + "]";
            }
        }

        public class Car
        {
            public string model;

            public Car(string model)
            {
                this.model = model;
            }

            public virtual string GetModel()
            {
                return model;
            }

            public override string ToString()
            {
                return "Car[" + model + "]";
            }
        }

        private sealed class _IDiagnosticListener_94 : IDiagnosticListener
        {
            private readonly IList classesNotFound;

            public _IDiagnosticListener_94(IList classesNotFound)
            {
                this.classesNotFound = classesNotFound;
            }

            public void OnDiagnostic(IDiagnostic d)
            {
                if (d is MissingClass)
                {
                    classesNotFound.Add(((MissingClass) d).Reason());
                }
            }
        }
    }
}

#endif // !SILVERLIGHT