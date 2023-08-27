using Common.Dtos;
using Common.Entities;

namespace Common.Mappings
{
    public class SharedMapping
    {
        public SharedMapping() { }

        /// <summary>
        /// Maps ApplicationUser to CreateApplicationUserResponse
        /// </summary>
        /// <param name="user">Map From</param>
        /// <param name="response">Mapped To</param>
        public void Map(ApplicationUser user, ApplicationUserResponseDto response) 
        {
            if(user != null && response != null)
            {
                response.UserId = user.UserId;
                response.UserName = user.UserName;
                response.UserGuid = user.UserGuid;
                response.Email = user.Email;
                response.IsLocked = user.IsUserLocked;                
            }
        }

        public void Map(ApplicationLayer applicationLayer, ApplicationLayerResponseDto response)
        {
            if (applicationLayer != null && response != null)
            {
                response.LayerId = applicationLayer.LayerId;
                response.LayerName = applicationLayer.LayerName;
                response.OwerId = applicationLayer.OwnerId;
                response.FilePath = applicationLayer.FilePath;
                response.Created = applicationLayer.CreateDate;
                response.Updated = applicationLayer.ModifiedDate;
            }
        }
    }
}
