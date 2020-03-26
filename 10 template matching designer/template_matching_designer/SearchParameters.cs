//
// Adaptive Vision Library .NET Example - "Template Matching Designer" example
//
// Simple demonstration applications which uses a Template Matching Designer
//
// Copyright (C) 2020 Adaptive Vision Sp. z o.o.
// Version: 4.12_74433
//

using System.ComponentModel;

namespace template_matching_designer
{
    // Example class presents convenient way
    // to work with setting parameters.

    public class SearchParameters
    {
        public SearchParameters()
        {
            IgnoreEdgesPolarity = false;
            EdgeThresholdLevel = 30.0f;
            MinimalDistances = 10.0f;
            MinimalScore = 0.7f;
            DrawResultsOnPreview = true;
        }

        [Category("Algorithm"),
        DescriptionAttribute("Denotes if algorithm should take into account edge polarity (direction).")]
        public bool IgnoreEdgesPolarity { get; set; }

        [Category("Algorithm"),
        DescriptionAttribute("Minimal edge threshold which will be checked for match.")]
        public float EdgeThresholdLevel { get; set; }

        [Category("Algorithm"),
        DescriptionAttribute("Minimal distance between two matches.")]
        public float MinimalDistances { get; set; }

        [Category("Algorithm"),
        DescriptionAttribute("Minimal acceptance score for results.")]
        public float MinimalScore { get; set; }

        [Category("Appearance"),
        DescriptionAttribute("Should results be drawn on an input image.")]
        public bool DrawResultsOnPreview { get; set; }
    }
}
