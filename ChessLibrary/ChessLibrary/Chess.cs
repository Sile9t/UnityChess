using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Chess
    {
        public string fen { get; private set; }
        Board board;
        Moves moves;
        List<FigureMoving> allMoves;
        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            this.fen = fen;
            board = new Board(fen);
            moves = new Moves(board);
        }
        Chess (Board board)
        {
            this.board = board;
            this.fen = board.fen;
            moves = new Moves(board);
        }
        public Chess Move(string move)  // Pe2e4   Pe7eBQ
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm))
                return this;
            if (board.IsCheckAfterMove(fm))
               return this;
            Board nextBoard = board.Move(fm);
            Chess nextChess = new Chess(nextBoard);
            return nextChess;
        }
        public char GetFigureAt(int x, int y)
        {
            Square square = new Square(x, y);
            Figure f = board.GetFigureAt(square);
            return f == Figure.none ? '.' : (char)f;
        }
        void FindAllMoves()
        {
            allMoves = new List<FigureMoving>();
            foreach (FigureOnSquare fs in board.YieldFigures())
                foreach (Square to in Square.YieldSquares())
                {
                    FigureMoving fm = new FigureMoving(fs, to);
                    if (moves.CanMove(fm))
                        if (!board.IsCheckAfterMove(fm))
                        allMoves.Add(fm);
                }
        }
        public List<string> GetAllMoves()
        {
            FindAllMoves();
            List<string> list = new List<string>();
            foreach(FigureMoving fm in allMoves)
                list.Add(fm.ToString());
            return list;
        }
        public bool IsCheck()
        {
            return board.IsCheck();
        }
    }
}
