﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BLL.DTO;
using BLL.Interface;
using DAL.Interface;
using DAL.Model;

namespace BLL.Service
{
    public class CrudService : ICrudService
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IRateRepository _rateRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CrudService(IFilmRepository fr, IGenreRepository gr, ICommentsRepository cr, ILogger<CrudService> logger, IRateRepository rr, IMapper mapper)
        {
            _filmRepository = fr;
            _genreRepository = gr;
            _commentsRepository = cr;
            _logger = logger;
            _rateRepository = rr;
            _mapper = mapper;
        }

        public async Task<CommentDTO> CreateCommentAsync(CommentDTO comment, ClaimsPrincipal user)
        {
            _logger.LogInformation("Start creating comment.");
            try
            {
                Comment c = _mapper.CommentDTOtoModel(comment);
                
                _logger.LogInformation("Call CreateCommentAsync.");
                c = await _commentsRepository.CreateCommentAsync(c, user);
                
                _logger.LogInformation($"Create comment: comment with Id equal {comment.Id} was created.");

                return _mapper.CommentModelToDTO(c);
            }
            catch (Exception e)
            {
                _logger.LogError($"Create comment has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<FilmDTO> CreateFilmAsync(FilmDTO film)
        {
            _logger.LogInformation("Start creating film.");
            try
            {
                Film f = _mapper.FilmDTOtoModel(film);
                
                _logger.LogInformation("Call CreateFilmAsync.");
                f = await _filmRepository.CreateFilmAsync(f);
                
                _logger.LogInformation($"Create film: film with Id equal {film.Id} was created.");

                return _mapper.FilmModelToDTO(f);
            }
            catch(Exception e)
            {
                _logger.LogError($"Create film has thrown an exception: {e.Message}.");
                
                return null;
            }
        }

        public async Task<RateDTO> CreateRateAsync(RateDTO rate, ClaimsPrincipal user)
        {
            _logger.LogInformation("Start creating rate.");
            try
            {
                Rate r = _mapper.RateDTOtoModel(rate);

                _logger.LogInformation("Call CreateRateAsync.");
                r = await _rateRepository.CreateRateAsync(r, user);

                _logger.LogInformation($"Create rate: rate with Id equal {rate.Id} was created.");

                return _mapper.RateModelToDTO(r);
            }
            catch (Exception e)
            {
                _logger.LogError($"Create rate has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task DeleteCommentAsync(int id)
        {
            _logger.LogInformation($"Start deleting comment with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call DeleteCommentAsync.");
                await _commentsRepository.DeleteCommentAsync(id);

                _logger.LogInformation($"Delete comment: comment with Id equal {id} was deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete comment with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task DeleteFilmAsync(int id)
        {
            _logger.LogInformation($"Start deleting film with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call DeleteFilmAsync.");
                await _filmRepository.DeleteFilmAsync(id);
                
                _logger.LogInformation($"Delete film: film with Id equal {id} was deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete film with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task DeleteRateAsync(int id)
        {
            _logger.LogInformation($"Start deleting rate with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call DeleteRateAsync.");
                await _rateRepository.DeleteRateAsync(id);

                _logger.LogInformation($"Delete rate: rate with Id equal {id} was deleted.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete rate with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public IEnumerable<GenreDTO> GetAllGenres()
        {
            _logger.LogInformation("Starting get all genres.");
            try
            {
                _logger.LogInformation("Call GetAllGenres.");
                var genres = _genreRepository.GetAllGenres();

                _logger.LogInformation("Get all genres successfull.");

                return genres.Select(e => _mapper.GenreModelToDTO(e)).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Get all genres has thrown an exception: {e.Message}.");

                return new List<GenreDTO>();
            }
        }

        public async Task<FilmDTO> GetFilmAsync(int id)
        {
            _logger.LogInformation($"Starting get film with Id equal {id}.");
            try
            {
                _logger.LogInformation("Call GetFilmAsync.");
                var film = await _filmRepository.GetFilmAsync(id);

                if (film == null)
                {
                    _logger.LogInformation($"Get film: film with Id equal {id} not found.");
                    
                    return null;
                }

                _logger.LogInformation($"Get film with Id equal {id} successfully.");

                return _mapper.FilmModelToDTO(film);
            }
            catch (Exception e)
            {
                _logger.LogError($"Get film with Id equal {id} has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<int?> GetFilmRate(int filmId, ClaimsPrincipal user)
        {
            _logger.LogInformation($"Start get film rate with id={filmId} of current user.");
            try
            {
                _logger.LogInformation("Call GetFilmRate.");
                int? res = await _rateRepository.GetFilmRate(filmId, user);

                string message = res == null
                    ? $"Get film rate of current user: rate of film with Id equal {filmId} not found."
                    : $"Get film rate of current user: Ok - {res}.";
                _logger.LogInformation(message);

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"Get rate of film with id={filmId} of current user has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task UpdateCommentAsync(int id, CommentDTO comment)
        {
            _logger.LogInformation($"Start updating comment with Id equal {id}.");
            try
            {
                Comment c = _mapper.CommentDTOtoModel(comment);

                _logger.LogInformation("Call UpdateCommentAsync.");
                await _commentsRepository.UpdateCommentAsync(id, c);
                
                _logger.LogInformation($"Update comment: comment with Id equal {id} was updated.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update comment with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task UpdateFilmAsync(int id, FilmDTO film)
        {
            _logger.LogInformation($"Start updating film with Id equal {id}.");
            try
            {
                Film f = _mapper.FilmDTOtoModel(film);
                
                _logger.LogInformation("Call UpdateFilmAsync.");
                await _filmRepository.UpdateFilmAsync(id, f);
                
                _logger.LogInformation($"Update film: film with Id equal {id} was updated.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update film with id={id} has thrown an exception: {e.Message}.");
            }
        }

        public async Task UpdateRateAsync(int id, RateDTO rate)
        {
            _logger.LogInformation($"Start updating rate with Id equal {id}.");
            try
            {
                Rate r = _mapper.RateDTOtoModel(rate);

                _logger.LogInformation("Call UpdateRateAsync.");
                await _rateRepository.UpdateRateAsync(id, r);

                _logger.LogInformation($"Update rate: rate with Id equal {id} was updated.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update rate with id={id} has thrown an exception: {e.Message}.");
            }
        }
    }
}
