using Find2Me.DAL;
using Find2Me.Infrastructure;
using Find2Me.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Find2Me.Services
{
    public interface ILogsSerivce
    {
    }
    public class LogsSerivce : ILogsSerivce
    {
        public void RunAddLogTask(string actionType, string fromUserId, string toUserId = null, string extraMessage = null)
        {
            string ipAdress = HttpContext.Current.Request.UserHostAddress;
            Task.Run(() => AddLog(actionType, fromUserId, toUserId, extraMessage, ipAdress));
        }

        public void RunAddLogTask(string[] actionTypes, string fromUserId, string toUserId = null, string extraMessage = null)
        {
            string ipAdress = HttpContext.Current.Request.UserHostAddress;
            Task.Run(() => AddLog(actionTypes, fromUserId, toUserId, extraMessage, ipAdress));
        }

        private ResponseResult<object> AddLog(string actionType, string fromUserId, string toUserId = null, string extraMessage = null, string ipAddress = null)
        {
            var response = new ResponseResult<object>
            {
                Success = true,
            };
            try
            {
                string actionMessage = _LogActionType.GetActionMessage(actionType) + " " + extraMessage;

                using (ApplicationDbContext dbContext = new ApplicationDbContext())
                {
                    LogsRepository logsRepository = new LogsRepository(dbContext);
                    logsRepository.Insert(new Logs
                    {
                        ActionType = actionType,
                        ActionMessage = actionMessage,
                        ActionFromUserId = fromUserId,
                        ActionToUserId = toUserId,
                        IpAddress = ipAddress,
                        CreateTime = DateTime.UtcNow,
                    });
                    logsRepository.SaveChanges();
                }
            }
            catch (Exception err)
            {
                response.Message = err.Message;
                response.Success = false;
            }

            return response;
        }

        private ResponseResult<object> AddLog(string[] actionTypes, string fromUserId, string toUserId = null, string extraMessage = null, string ipAddress = null)
        {
            var response = new ResponseResult<object>
            {
                Success = true,
            };
            try
            {

                using (ApplicationDbContext dbContext = new ApplicationDbContext())
                {
                    LogsRepository logsRepository = new LogsRepository(dbContext);
                    List<Logs> logs = new List<Logs>();
                    foreach (var actionType in actionTypes)
                    {
                        string actionMessage = _LogActionType.GetActionMessage(actionType) + " " + extraMessage;
                        Logs log = new Logs
                        {
                            ActionType = actionType,
                            ActionMessage = actionMessage,
                            ActionFromUserId = fromUserId,
                            ActionToUserId = toUserId,
                            IpAddress = ipAddress,
                            CreateTime = DateTime.UtcNow,
                        };
                    }
                    logsRepository.Insert(logs);
                    logsRepository.SaveChanges();
                }
            }
            catch (Exception err)
            {
                response.Message = err.Message;
                response.Success = false;
            }

            return response;
        }
    }
}
