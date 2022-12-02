using System;
using aoc_2022_cli.Entities;

namespace aoc_2022_cli.Helpers;

public class RockPaperScissors
{
    public static RPSMoves ConvertMove(string move)
    {
        switch (move)
        {
            case "A":
            case "X":
                return RPSMoves.Rock;
            case "B":
            case "Y":
                return RPSMoves.Paper;
            case "C":
            case "Z":
                return RPSMoves.Scissors;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static Result ConvertResult(string move)
    {
        switch (move)
        {
            case "X":
                return Result.Loss;
            case "Y":
                return Result.Draw;
            case "Z":
                return Result.Win;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static Round GetMyRoundResults(RPSMoves myMove, RPSMoves elfMove)
    {
        // I draw
        if (myMove == elfMove)
        {
            var shapePts = ComputeShapePoints(myMove);
            var resultPts = ComputeResultPoints(Result.Draw);

            Round round = new Round()
            {
                Move = myMove,
                Result = Result.Draw,
                Points = shapePts + resultPts,
            };
            return round;
        }
        // I win
        else if ((myMove == RPSMoves.Rock && elfMove == RPSMoves.Scissors) ||
                 (myMove == RPSMoves.Scissors && elfMove == RPSMoves.Paper) ||
                 (myMove == RPSMoves.Paper && elfMove == RPSMoves.Rock))
        {
            var shapePts = ComputeShapePoints(myMove);
            var resultPts = ComputeResultPoints(Result.Win);

            Round round = new Round()
            {
                Move = myMove,
                Result = Result.Win,
                Points = shapePts + resultPts,
            };
            return round;
        }
        // I lose
        else
        {
            var shapePts = ComputeShapePoints(myMove);
            var resultPts = ComputeResultPoints(Result.Loss);

            Round round = new Round()
            {
                Move = myMove,
                Result = Result.Loss,
                Points = shapePts + resultPts,
            };
            return round;
        }
    }

    // this is ugly...
    public static Round GetMyRound2Results(RPSMoves elfMove, Result outcome)
    {
        if (outcome == Result.Win)
        {
            if (elfMove == RPSMoves.Rock)
            {
                var myMove = RPSMoves.Paper;
                var shapePts = ComputeShapePoints(myMove);
                var resultPts = ComputeResultPoints(outcome);

                Round round = new Round()
                {
                    Move = myMove,
                    Result = outcome,
                    Points = shapePts + resultPts,
                };
                return round;
            }
            else if (elfMove == RPSMoves.Paper)
            {
                var myMove = RPSMoves.Scissors;
                var shapePts = ComputeShapePoints(myMove);
                var resultPts = ComputeResultPoints(outcome);

                Round round = new Round()
                {
                    Move = myMove,
                    Result = outcome,
                    Points = shapePts + resultPts,
                };
                return round;
            }
            else // if (elfMove == RPSMoves.Scissors)
            {
                var myMove = RPSMoves.Rock;
                var shapePts = ComputeShapePoints(myMove);
                var resultPts = ComputeResultPoints(outcome);

                Round round = new Round()
                {
                    Move = myMove,
                    Result = outcome,
                    Points = shapePts + resultPts,
                };
                return round;
            }
        }
        else if (outcome == Result.Draw)
        {
            if (elfMove == RPSMoves.Rock)
            {
                var myMove = RPSMoves.Rock;
                var shapePts = ComputeShapePoints(myMove);
                var resultPts = ComputeResultPoints(outcome);

                Round round = new Round()
                {
                    Move = myMove,
                    Result = outcome,
                    Points = shapePts + resultPts,
                };
                return round;
            }
            else if (elfMove == RPSMoves.Paper)
            {
                var myMove = RPSMoves.Paper;
                var shapePts = ComputeShapePoints(myMove);
                var resultPts = ComputeResultPoints(outcome);

                Round round = new Round()
                {
                    Move = myMove,
                    Result = outcome,
                    Points = shapePts + resultPts,
                };
                return round;
            }
            else // if (elfMove == RPSMoves.Scissors)
            {
                var myMove = RPSMoves.Scissors;
                var shapePts = ComputeShapePoints(myMove);
                var resultPts = ComputeResultPoints(outcome);

                Round round = new Round()
                {
                    Move = myMove,
                    Result = outcome,
                    Points = shapePts + resultPts,
                };
                return round;
            }
        }
        else // if (outcome == Result.Loss)
        {
            if (elfMove == RPSMoves.Rock)
            {
                var myMove = RPSMoves.Scissors;
                var shapePts = ComputeShapePoints(myMove);
                var resultPts = ComputeResultPoints(outcome);

                Round round = new Round()
                {
                    Move = myMove,
                    Result = outcome,
                    Points = shapePts + resultPts,
                };
                return round;
            }
            else if (elfMove == RPSMoves.Paper)
            {
                var myMove = RPSMoves.Rock;
                var shapePts = ComputeShapePoints(myMove);
                var resultPts = ComputeResultPoints(outcome);

                Round round = new Round()
                {
                    Move = myMove,
                    Result = outcome,
                    Points = shapePts + resultPts,
                };
                return round;
            }
            else // if (elfMove == RPSMoves.Scissors)
            {
                var myMove = RPSMoves.Paper;
                var shapePts = ComputeShapePoints(myMove);
                var resultPts = ComputeResultPoints(outcome);

                Round round = new Round()
                {
                    Move = myMove,
                    Result = outcome,
                    Points = shapePts + resultPts,
                };
                return round;
            }
        }
    }

    private static int ComputeShapePoints(RPSMoves move)
    {
        // 1 pt for rock win
        // 2 pts for paper win
        // 3 pts for scissors win

        switch (move)
        {
            case RPSMoves.Rock:
                return 1;
            case RPSMoves.Paper:
                return 2;
            case RPSMoves.Scissors:
                return 3;
            default:
                return 0;
        }
    }

    private static int ComputeResultPoints(Result result)
    {
        switch (result)
        {
            case Result.Win:
                return 6;
            case Result.Draw:
                return 3;
            case Result.Loss:
            default:
                return 0;
        }
    }

}

