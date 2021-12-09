using System.Collections.Generic;

namespace DotNetNote.Models
{
    public interface IInquiryCommentRepository
    {
        void AddInquiryComment(InquiryComment model);
        int DeleteInquiryComment(int boardId, int id, string password);
        int GetCountBy(int boardId, int id, string password);
        List<InquiryComment> GetInquiryComments(int boardId);
        List<InquiryComment> GetRecentComments();
        List<InquiryComment> GetRecentCommentsNoCache();
    }
}