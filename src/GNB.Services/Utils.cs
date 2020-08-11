using System;

namespace GNB.Services
{
    public static class Utils
    {
        //default bank round: MidpointRounding.ToEven
        public static decimal BankRound(decimal amount, int decimalPlaces = 2) => Math.Round(amount, decimalPlaces);
    }
}
