using System;

namespace SquashNiagaraLib
{
    public class RankPlayer
    {

        public static double CalcPoints(string position, short HomePlayerScore, short AwayPlayerScore, ResultFor playerType)
        {

            if (HomePlayerScore == 3 && AwayPlayerScore == 0)
            {
                if (position == "Position 1")
                {
                    if (playerType == ResultFor.Home)
                        return 10d;
                    else
                        return 7.5d;
                }
                else if (position == "Position 2")
                {
                    if (playerType == ResultFor.Home)
                        return 9d;
                    else
                        return 6.5d;

                }
                else if (position == "Position 3")
                {
                    if (playerType == ResultFor.Home)
                        return 8d;
                    else
                        return 5.5d;
                }
                else if (position == "Position 4")
                {
                    if (playerType == ResultFor.Home)
                        return 7d;
                    else
                        return 4.5d;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid position");
                }

            }
            else if (HomePlayerScore == 3 && AwayPlayerScore == 1)
            {
                if (position == "Position 1")
                {
                    if (playerType == ResultFor.Home)
                        return 9.5d;
                    else
                        return 8d;
                }
                else if (position == "Position 2")
                {
                    if (playerType == ResultFor.Home)
                        return 8.5d;
                    else
                        return 7d;

                }
                else if (position == "Position 3")
                {
                    if (playerType == ResultFor.Home)
                        return 7.5d;
                    else
                        return 6d;
                }
                else if (position == "Position 4")
                {
                    if (playerType == ResultFor.Home)
                        return 6.5d;
                    else
                        return 5d;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid position");
                }
            }
            else if (HomePlayerScore == 3 && AwayPlayerScore == 2)
            {
                if (position == "Position 1")
                {
                    if (playerType == ResultFor.Home)
                        return 9d;
                    else
                        return 8.5d;
                }
                else if (position == "Position 2")
                {
                    if (playerType == ResultFor.Home)
                        return 8d;
                    else
                        return 7.5d;

                }
                else if (position == "Position 3")
                {
                    if (playerType == ResultFor.Home)
                        return 7d;
                   else
                        return 6.5d;
                }
                else if (position == "Position 4")
                {
                    if (playerType == ResultFor.Home)
                        return 6d;
                    else
                        return 5.5d;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid position");
                }
            }//end Home
            if (HomePlayerScore == 0 && AwayPlayerScore == 3)
            {
                if (position == "Position 1")
                {
                    if (playerType == ResultFor.Away)
                        return 10d;
                    else
                        return 7.5d;
                }
                else if (position == "Position 2")
                {
                    if (playerType == ResultFor.Away)
                        return 9d;
                    else
                        return 6.5d;

                }
                else if (position == "Position 3")
                {
                    if (playerType == ResultFor.Away)
                        return 8d;
                    else
                        return 5.5d;
                }
                else if (position == "Position 4")
                {
                    if (playerType == ResultFor.Away)
                        return 7d;
                    else
                        return 4.5d;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid position");
                }

            }
            else if (HomePlayerScore == 1 && AwayPlayerScore == 3)
            {
                if (position == "Position 1")
                {
                    if (playerType == ResultFor.Away)
                        return 9.5d;
                    else
                        return 8d;
                }
                else if (position == "Position 2")
                {
                    if (playerType == ResultFor.Away)
                        return 8.5d;
                    else
                        return 7d;

                }
                else if (position == "Position 3")
                {
                    if (playerType == ResultFor.Away)
                        return 7.5d;
                    else
                        return 6d;
                }
                else if (position == "Position 4")
                {
                    if (playerType == ResultFor.Away)
                        return 6.5d;
                    else
                        return 5d;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid position");
                }
            }
            else if (HomePlayerScore == 2 && AwayPlayerScore == 3)
            {
                if (position == "Position 1")
                {
                    if (playerType == ResultFor.Away)
                        return 9d;
                    else
                        return 8.5d;
                }
                else if (position == "Position 2")
                {
                    if (playerType == ResultFor.Away)
                        return 8d;
                    else
                        return 7.5d;

                }
                else if (position == "Position 3")
                {
                    if (playerType == ResultFor.Away)
                        return 7d;
                    else
                        return 6.5d;
                }
                else if (position == "Position 4")
                {
                    if (playerType == ResultFor.Away)
                        return 6d;
                    else
                        return 5.5d;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid position");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid scores");
            }
        }



        //public static void CalcPoints (int position, int winnerScore, int loserPlayerScore, out double winnerPlayerPoints, out double loserPlayerPoints)
        //{
        //    winnerPlayerPoints = 0;
        //    loserPlayerPoints = 0;

        //    if (winnerScore < 0 ||
        //        winnerScore > 3 ||
        //        loserPlayerScore < 0 ||
        //        loserPlayerScore > 3 ||
        //        position < 1 ||
        //        position > 5)
        //    {
        //        throw new ArgumentOutOfRangeException("Invalid scores");
        //    }
        //    else if (winnerScore == 3 && loserPlayerScore == 0)
        //    {
        //        if (position == 1)
        //        {
        //            winnerPlayerPoints = 10d;
        //            loserPlayerPoints = 7.5d;
        //        }
        //        else if (position == 2)
        //        {
        //            winnerPlayerPoints = 9d;
        //            loserPlayerPoints = 6.5d;

        //        }
        //        else if (position == 3)
        //        {
        //            winnerPlayerPoints = 8d;
        //            loserPlayerPoints = 5.5d;
        //        }
        //        else if (position == 4)
        //        {
        //            winnerPlayerPoints = 7d;
        //            loserPlayerPoints = 4.5d;
        //        } else
        //        {
        //            throw new ArgumentOutOfRangeException("Invalid position");
        //        }

        //    }
        //    else if (winnerScore == 3 && loserPlayerScore == 1)
        //    {
        //        if (position == 1)
        //        {
        //            winnerPlayerPoints = 9.5d;
        //            loserPlayerPoints = 8d;
        //        }
        //        else if (position == 2)
        //        {
        //            winnerPlayerPoints = 8.5d;
        //            loserPlayerPoints = 7d;

        //        }
        //        else if(position == 3)
        //        {
        //            winnerPlayerPoints = 7.5d;
        //            loserPlayerPoints = 6d;
        //        }
        //        else if (position == 4)
        //        {
        //            winnerPlayerPoints = 6.5d;
        //            loserPlayerPoints = 5d;
        //        }
        //        else
        //        {
        //            throw new ArgumentOutOfRangeException("Invalid position");
        //        }
        //    }
        //    else if (winnerScore == 3 && loserPlayerScore == 2)
        //    {
        //        if (position == 1)
        //        {
        //            winnerPlayerPoints = 9d;
        //            loserPlayerPoints = 8.5d;
        //        }
        //         else if (position == 2)
        //        {
        //            winnerPlayerPoints = 8d;
        //            loserPlayerPoints = 7.5d;

        //        }
        //        else if (position == 3)
        //        {
        //            winnerPlayerPoints = 7d;
        //            loserPlayerPoints = 6.5d;
        //        }
        //        else if (position == 4)
        //        {
        //            winnerPlayerPoints = 6d;
        //            loserPlayerPoints = 5.5d;
        //        }
        //        else
        //        {
        //            throw new ArgumentOutOfRangeException("Invalid position");
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentOutOfRangeException("Invalid scores");
        //    }
        //}


    }
}
