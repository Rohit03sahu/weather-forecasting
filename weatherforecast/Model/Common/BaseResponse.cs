namespace weatherforecast.Model.Common
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public List<string> reason { get; set; }
    }
}
