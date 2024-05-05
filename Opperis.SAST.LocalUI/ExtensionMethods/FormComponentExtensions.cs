using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.LocalUI.ExtensionMethods;

internal static class FormComponentExtensions
{
    internal static void UpdateText(this Label label, string text)
    {
        label.Text = text;
        label.Refresh();
    }

    internal static void UpdatePercentComplete(this Label label, int numerator, int denominator)
    {
        label.Text = (numerator + 1).AsPercentage(denominator);
        label.Refresh();
    }

    internal static void UpdatePercentComplete(this Label label, int primaryNumerator, int primaryDenominator, int secondaryNumerator, int secondaryDenominator)
    {
        var primaryIncrementRate = 1.0 / primaryDenominator;
        var secondaryAmount = primaryIncrementRate * (secondaryNumerator + 1) / secondaryDenominator;
        var amount = (primaryNumerator / (float)primaryDenominator + secondaryAmount) * 100.0;

        //Correct rounding error
        amount = amount > 100.0 ? 100.0 : amount;
        label.Text = amount.ToString("##.#\\%");
        label.Refresh();
    }

    internal static void UpdateFindingCount(this Label label, int count)
    {
        label.Text = $"Findings: {count}";
        label.Refresh();
    }
}
