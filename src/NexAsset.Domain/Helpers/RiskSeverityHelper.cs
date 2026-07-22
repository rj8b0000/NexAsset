using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Helpers;

/// <summary>
/// Computes risk severity from a 3x3 probability-impact matrix.
/// </summary>
/// <remarks>
/// Matrix (Probability × Impact → Severity):
/// <code>
///             Low       Medium    High
/// Low      →  Low       Low       Medium
/// Medium   →  Low       Medium    High
/// High     →  Medium    High      Critical
/// </code>
/// </remarks>
public static class RiskSeverityHelper
{
    /// <summary>
    /// Computes the risk severity based on probability and impact using the defined 3x3 matrix.
    /// </summary>
    /// <param name="p">Risk probability (Low, Medium, High)</param>
    /// <param name="i">Risk impact (Low, Medium, High)</param>
    /// <returns>Computed <see cref="RiskSeverity"/> value.</returns>
    public static RiskSeverity ComputeSeverity(RiskProbability p, RiskImpact i) => (p, i) switch
    {
        (RiskProbability.Low,    RiskImpact.Low)    => RiskSeverity.Low,
        (RiskProbability.Low,    RiskImpact.Medium) => RiskSeverity.Low,
        (RiskProbability.Low,    RiskImpact.High)   => RiskSeverity.Medium,
        (RiskProbability.Medium, RiskImpact.Low)    => RiskSeverity.Low,
        (RiskProbability.Medium, RiskImpact.Medium) => RiskSeverity.Medium,
        (RiskProbability.Medium, RiskImpact.High)   => RiskSeverity.High,
        (RiskProbability.High,   RiskImpact.Low)    => RiskSeverity.Medium,
        (RiskProbability.High,   RiskImpact.Medium) => RiskSeverity.High,
        (RiskProbability.High,   RiskImpact.High)   => RiskSeverity.Critical,
        _                                           => RiskSeverity.Low
    };
}
