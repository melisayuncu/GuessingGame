using System;
using System.Collections.Generic;

namespace WellTopVisualization.Models;

public partial class Guess
{
    public int GuessId { get; set; }

    public int TakenNumber { get; set; }

    public int Difference { get; set; }

    public int TotalWin { get; set; }
}
