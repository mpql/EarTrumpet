﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace EarTrumpet_Actions.DataModel
{
    public class OptionData
    {
        public List<Option> Options { get; }

        public Option Selected
        {
            get => Options.First(o => o.Value.Equals(GetSelected()));
            set => SetSelected(value);
        }

        public Action<Option> SetSelected { get; }
        public Func<object> GetSelected { get; }

        public string DisplayName => Selected.DisplayName;

        public OptionData(List<Option> options, Action<Option> setSelected, Func<object> getSelected)
        {
            Options = options;
            SetSelected = setSelected;
            GetSelected = getSelected;
        }
    }

    public class PartWithOptions : Part
    {
        public override string Describe()
        {
            throw new NotImplementedException();
        }

        [XmlIgnore]
        public List<OptionData> Options { get; protected set; }
    }
}
