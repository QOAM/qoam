﻿namespace QOAM.Core
{
    public class Price
    {
        public decimal? Amount { get; set; }
        public Currency? Currency { get; set; }
        public FeeType? FeeType { get; set; }
    }
}