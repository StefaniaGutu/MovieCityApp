using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.Account;
using MovieCity.BusinessLogic.Implementation.FriendImp.Models;
using MovieCity.Entities;
using MovieCity.Entities.Enums;
using X.PagedList;

namespace MovieCity.BusinessLogic.Implementation.FriendImp
{
    public class FriendService : BaseService
    {
        private readonly UserAccountService userService;
        public FriendService(ServiceDependencies serviceDependencies, UserAccountService userService) : base(serviceDependencies)
        {
            this.userService = userService;
        }

        public async Task UpdateStatus(Guid receiverId, ActionTypes action)
        {
            switch (action)
            {
                case ActionTypes.Send:
                    {
                        var request = new FriendRequest
                        {
                            ReceiverId = receiverId,
                            SenderId = CurrentUser.Id,
                            Date = DateTime.Now
                        };

                        UnitOfWork.FriendRequests.Insert(request);
                    }
                    break;
                case ActionTypes.Cancel:
                    {
                        var request = await UnitOfWork.FriendRequests.Get()
                            .FirstOrDefaultAsync(r => r.SenderId == CurrentUser.Id && r.ReceiverId == receiverId);
                        if (request != null)
                            UnitOfWork.FriendRequests.Delete(request);
                    }
                    break;
                case ActionTypes.Accept:
                    {
                        var request = await UnitOfWork.FriendRequests.Get()
                            .FirstOrDefaultAsync(r => r.SenderId == receiverId && r.ReceiverId == CurrentUser.Id);
                        if (request != null)
                            UnitOfWork.FriendRequests.Delete(request);

                        var friend = new Friend
                        {
                            User2Id = receiverId,
                            User1Id = CurrentUser.Id,
                            Date = DateTime.Now
                        };

                        UnitOfWork.Friends.Insert(friend);
                    }
                    break;
                case ActionTypes.Decline:
                    {
                        var request = await UnitOfWork.FriendRequests.Get()
                            .FirstOrDefaultAsync(r => r.ReceiverId == CurrentUser.Id && r.SenderId == receiverId);
                        if(request != null)
                            UnitOfWork.FriendRequests.Delete(request);
                    }
                    break;
                default: //Remove
                    {
                        var friend = await UnitOfWork.Friends.Get()
                            .FirstOrDefaultAsync(r => (r.User1Id == CurrentUser.Id && r.User2Id == receiverId) || (r.User2Id == CurrentUser.Id && r.User1Id == receiverId));
                        if(friend != null)
                            UnitOfWork.Friends.Delete(friend);
                    }
                    break;
            }
            await UnitOfWork.SaveChangesAsync();
        }

        public IPagedList<ListFriendsModel> GetFriends(int pageNumber, int pageSize, string? searchString = null, Guid? userId = null)
        {
            if (userId == null)
            {
                userId = CurrentUser.Id;
            }

            var friends = UnitOfWork.Friends.Get()
                .Include(s => s.User2)
                .Include(s => s.User1)
                .Where(f => (f.User1Id == userId && !f.User2.IsDeleted) ||  (f.User2Id == userId && !f.User1.IsDeleted));

            if (!string.IsNullOrEmpty(searchString))
            {
                friends = friends.Where(f => f.User2.Username.Contains(searchString));
            }

            return friends.Select(s => new ListFriendsModel
                        {
                            Id = s.User1Id == CurrentUser.Id ? s.User2Id : s.User1Id,
                            Username = s.User1Id == CurrentUser.Id ? s.User2.Username : s.User1.Username,
                            Date = s.Date,
                            Image = s.User1Id == CurrentUser.Id 
                                ? s.User2.UserImage != null 
                                    ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(s.User2.UserImage.Image)) 
                                    : userService.GetDefaultUserImage()
                                : s.User1.UserImage != null 
                                    ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(s.User1.UserImage.Image)) 
                                    : userService.GetDefaultUserImage()
            }).ToPagedList(pageNumber, pageSize);
        }

        public async Task<IPagedList<ListFriendsModel>> GetSentRequests(string searchString, int pageNumber, int pageSize)
        {
            var sentRequests = UnitOfWork.FriendRequests.Get()
                .Include(s => s.Receiver)
                .Where(r => r.SenderId == CurrentUser.Id && !r.Receiver.IsDeleted);

            if (!string.IsNullOrEmpty(searchString))
            {
                sentRequests = sentRequests.Where(r => r.Receiver.Username.Contains(searchString));
            }

            return await sentRequests
                .Select(s => new ListFriendsModel
                {
                    Id = s.ReceiverId,
                    Username = s.Receiver.Username,
                    Date = s.Date,
                    Image = s.Receiver.UserImage != null 
                        ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(s.Receiver.UserImage.Image)) 
                        : userService.GetDefaultUserImage()
                }).ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<IPagedList<ListFriendsModel>> GetReceivedRequests(string searchString, int pageNumber, int pageSize)
        {
            var receivedRequests = UnitOfWork.FriendRequests.Get()
                .Include(s => s.Sender)
                .Where(r => r.ReceiverId == CurrentUser.Id && !r.Sender.IsDeleted);

            if (!string.IsNullOrEmpty(searchString))
            {
                receivedRequests = receivedRequests.Where(r => r.Sender.Username.Contains(searchString));
            }

            return await receivedRequests
                .Select(s => new ListFriendsModel
                {
                    Id = s.SenderId,
                    Username = s.Sender.Username,
                    Date = s.Date,
                    Image = s.Sender.UserImage != null 
                        ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(s.Sender.UserImage.Image)) 
                        : userService.GetDefaultUserImage()
                })
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<List<ListFriendsModel>> GetRecentReceivedRequests()
        {
            var receivedRequests = UnitOfWork.FriendRequests.Get()
                .Include(s => s.Sender)
                .Where(r => r.ReceiverId == CurrentUser.Id && !r.Sender.IsDeleted)
                .OrderByDescending(u => u.Date)
                .Take(5);

            return await receivedRequests
                .Select(s => new ListFriendsModel
                {
                    Id = s.SenderId,
                    Username = s.Sender.Username,
                    Date = s.Date,
                    Image = s.Sender.UserImage != null 
                        ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(s.Sender.UserImage.Image)) 
                        : userService.GetDefaultUserImage()
                })
                .ToListAsync(); ;
        }

        public async Task<FriendSuggestionsAndSearchUsersModel> GetFriendSuggestionsAndUsersByUsername(string searchString)
        {
            var model = new FriendSuggestionsAndSearchUsersModel();

            if (searchString != null)
            {
                model.SearchUsers = await GetUsersByUsername(searchString);
            }

            model.FriendsSuggestions = await GetFriendSuggestions();

            return model;
        }

        private async Task<List<ListFriendsModel>> GetUsersByUsername(string searchString)
        {
            return await UnitOfWork.Users.Get()
                .Where(u => u.Id != CurrentUser.Id && u.Username.Contains(searchString) && !u.IsDeleted)
                .Select(u => new ListFriendsModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    Image = u.UserImage != null 
                        ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(u.UserImage.Image)) 
                        : userService.GetDefaultUserImage()
                })
                .ToListAsync();
        }

        public List<Guid> GetFriendsIds()
        {
            return  UnitOfWork.Friends.Get()
                .Include(u => u.User1)
                .Include(u => u.User2)
                .Where(f => !f.User2.IsDeleted && !f.User1.IsDeleted && (f.User1.Id == CurrentUser.Id || f.User2.Id == CurrentUser.Id))
                .Select(f => f.User1Id == CurrentUser.Id ? f.User2Id : f.User1Id)
                .ToList();
        }

        private async Task<List<ListFriendsModel>> GetFriendSuggestions()
        {
            var userFriendsIds = GetFriendsIds();

            var friends = await UnitOfWork.Friends.Get()
                .Include(s => s.User1)
                    .ThenInclude(u => u.UserImage)
                .Include(s => s.User2)
                    .ThenInclude(u => u.UserImage)
                .Where(u => ((userFriendsIds.Contains(u.User2Id) && !userFriendsIds.Contains(u.User1Id)) 
                    || (!userFriendsIds.Contains(u.User2Id) && userFriendsIds.Contains(u.User1Id))) 
                    && u.User1Id != CurrentUser.Id && u.User2Id != CurrentUser.Id && !u.User1.IsDeleted && !u.User2.IsDeleted)
                    .Select(s => new ListFriendsModel
                    {
                        Id = userFriendsIds.Contains(s.User2Id) ? s.User1.Id : s.User2.Id,
                        Username = userFriendsIds.Contains(s.User2Id) ? s.User1.Username : s.User2.Username,
                        Image = userFriendsIds.Contains(s.User2Id) 
                            ? s.User1.UserImage != null
                                ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(s.User1.UserImage.Image))
                                : userService.GetDefaultUserImage()
                            : s.User2.UserImage != null ?
                                string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(s.User2.UserImage.Image))
                                : userService.GetDefaultUserImage()

                    })
                .ToListAsync();

            var friendSuggestions = friends.GroupBy(ab => (ab.Id, ab)).OrderByDescending(g => g.Count()).Take(3).Select(a => a.Key.ab).ToList();

            return friendSuggestions;
        }
    }
}
