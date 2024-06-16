using System;
using IPE.SmsIrClient.Models.Requests;
using IPE.SmsIrClient.Models.Results;
using IPE.SmsIrClient;
using MainProject.Features.Core.Application.IServices;
using Features.User.Domain.Entities;

namespace MainProject.Features.Core.Application.Services
{
    public class SmsService : ISmsService
    {
        public async Task SendAsync(ApplicationUser user)
        {
            var smsIr = new SmsIr("temp");


            var templateId = 791511;
            VerifySendParameter[] verifySendParameters = {
            new VerifySendParameter("NAME", user.FirstName!=null && user.FirstName != string.Empty?user.FirstName:"کاربر گرامی"),
            // new VerifySendParameter("CODE", user.OTP_Code.ToString()),
            new VerifySendParameter("CODE", "OTP_Code"),
        };

            var response = await smsIr.VerifySendAsync(user.PhoneNumber, templateId, verifySendParameters);

            var sendResult = response.Data;
            var messageId = sendResult.MessageId;
            var cost = sendResult.Cost;
        }
    }
}

