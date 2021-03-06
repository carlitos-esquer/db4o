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
using System;
using Db4objects.Db4o.Activation;

namespace Db4odoc.Tutorial.F1.Chapter8
{
    public class TemperatureSensorReadout : SensorReadout
    {
        private readonly double _temperature;

        public TemperatureSensorReadout(DateTime time, Car car, string description, double temperature)
            : base(time, car, description)
        {
            this._temperature = temperature;
        }

        public double Temperature
        {
            get
            {
				Activate(ActivationPurpose.Read);
                return _temperature;
            }
        }

        public override String ToString()
        {
			Activate(ActivationPurpose.Read);
            return string.Format("{0} temp : {1}", base.ToString(), _temperature);
        }
    }
}