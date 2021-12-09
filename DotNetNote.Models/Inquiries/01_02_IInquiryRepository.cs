using System.Collections.Generic;

namespace DotNetNote.Models
{
    /// <summary>
    /// [1][2] 리포지토리 인터페이스 
    /// </summary>
    public interface IInquiryRepository
    {
        // 게시판에서 사용
        void Add(Inquiry model);
        int DeleteInquiry(int id, string password);
        List<Inquiry> GetAll(int page);
        int GetCountAll();
        int GetCountBySearch(string searchField, string searchQuery);
        string GetFileNameById(int id);
        Inquiry GetInquiryById(int id);
        List<Inquiry> GetSeachAll(
            int page, string searchField, string searchQuery);
        void ReplyInquiry(Inquiry model);
        void UpdateDownCount(string fileName);
        void UpdateDownCountById(int id);
        int UpdateInquiry(Inquiry model);
        
        // 메인 페이지에서 사용
        List<Inquiry> GetRecentPosts();
        List<Inquiry> GetRecentPostsCache();
        List<Inquiry> GetNewPhotos();
        List<Inquiry> GetNewPhotosCache();
        List<Inquiry> GetInquirySummaryByCategory(string category);
        List<Inquiry> GetInquirySummaryByCategoryCache(string category);
        List<Inquiry> GetInquirySummaryByCategoryBlog(string category);

        // 관리자 기능(페이지)에서 사용
        void Pinned(int id);
    }
}
