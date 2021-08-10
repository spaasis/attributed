﻿// Copyright 2015-2018 Destructurama Contributors, Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Serilog.Core;
using Serilog.Events;
using System;

namespace Destructurama.Attributed
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LogWithNameAttribute : Attribute, IPropertyDestructuringAttribute
    {
        public LogWithNameAttribute(string newName)
        {
            PropertyName = newName;
        }

        public string PropertyName { get; set; }

        public bool TryCreateLogEventProperty(string name, object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventProperty property)
        {
            var propValue = propertyValueFactory.CreatePropertyValue(value);

            LogEventPropertyValue logEventPropVal = propValue switch
            {
                ScalarValue scalar => new ScalarValue(scalar.Value),
                DictionaryValue dictionary => new DictionaryValue(dictionary.Elements),
                SequenceValue sequence => new SequenceValue(sequence.Elements),
                StructureValue structure => new StructureValue(structure.Properties),
                _ => null
            };

            property = new LogEventProperty(PropertyName, logEventPropVal);
            return true;
        }
    }
}