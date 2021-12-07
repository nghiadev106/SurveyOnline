using Newtonsoft.Json;

namespace SurveyOnline.API.Helpers
{
    public class ApiResponse
    {
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return "Nguồn tài nguyên không được tìm thấy";
                case 401:
                    return "Tài khoản chưa được xác thực";
                case 403:
                    return "Bạn không có quyền truy cập";
                case 500:
                    return "Đã xảy ra lỗi chưa được khắc phục";

                default:
                    return null;
            }
        }
    }
}
