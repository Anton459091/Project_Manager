using System;
using System.Data.Entity;
using System.Linq;
using Project_Manager.Data;

public class BoardRepository
{
    private readonly ProjectManagerEntities _context;

    public BoardRepository(ProjectManagerEntities context)
    {
        _context = context;
    }

    // Создание новой доски
    public Board CreateBoard(string title, int userId)
    {
        var board = new Board
        {
            Title = title,
            CreatedAt = DateTime.Now,
            Position = 0
        };

        var userBoard = new UserBoard
        {
            Users_ID = userId,
            Boards_ID = board.Boards_ID,
            User = _context.User.Find(userId),
            Board = board
        };

        _context.Board.Add(board);
        _context.UserBoard.Add(userBoard);
        _context.SaveChanges();

        return board;
    }

    // Полная загрузка доски с каталогами и карточками
    public Board GetFullBoard(int boardId)
    {
        return _context.Board
            .Include(b => b.Catalogs.Select(c => c.Cards))
            .FirstOrDefault(b => b.Boards_ID == boardId);
    }

    // Сохранение всей доски (транзакционно)
    public void SaveBoard(Board board)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                // Обновляем саму доску
                if (board.Boards_ID == 0)
                {
                    _context.Board.Add(board);
                }
                else
                {
                    _context.Entry(board).State = EntityState.Modified;
                }

                // Обрабатываем каталоги
                foreach (var catalog in board.Catalogs.ToList())
                {
                    if (catalog.Catalog_ID == 0)
                    {
                        _context.Catalog.Add(catalog);
                    }
                    else
                    {
                        _context.Entry(catalog).State = EntityState.Modified;
                    }

                    // Обрабатываем карточки
                    foreach (var card in catalog.Cards.ToList())
                    {
                        if (card.Cards_ID == 0)
                        {
                            _context.Card.Add(card);
                        }
                        else
                        {
                            _context.Entry(card).State = EntityState.Modified;
                        }
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    // Удаление доски
    public void DeleteBoard(int boardId)
    {
        var board = _context.Board
            .Include(b => b.Catalogs.Select(c => c.Cards))
            .FirstOrDefault(b => b.Boards_ID == boardId);

        if (board != null)
        {
            _context.Board.Remove(board);
            _context.SaveChanges();
        }
    }
}