using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DotNetNote.Models
{
    public class InquiryCommentRepository : IInquiryCommentRepository
    {
        private IDbConnection con; // SqlConnection에서 IDbConnection으로 변경
        private IMemoryCache _cache;
        private IConfiguration _config;

        public InquiryCommentRepository(IConfiguration config, IMemoryCache memoryCache)
        {
            _config = config;
            con = new SqlConnection(_config.GetSection("ConnectionStrings")
                .GetSection("DefaultConnection").Value);

            _cache = memoryCache;
        }

        /// <summary>
        /// 특정 게시물에 댓글 추가
        /// </summary>
        public void AddInquiryComment(InquiryComment model)
        {
            // 파라미터 추가
            var parameters = new DynamicParameters();
            parameters.Add(
                "@BoardId", value: model.BoardId, dbType: DbType.Int32);
            parameters.Add(
                "@Name", value: model.Name, dbType: DbType.String);
            parameters.Add(
                "@Opinion", value: model.Opinion, dbType: DbType.String);
            parameters.Add(
                "@Password", value: model.Password, dbType: DbType.String);

            string sql = @"
                Insert Into InquiryComments (BoardId, Name, Opinion, Password)
                Values(@BoardId, @Name, @Opinion, @Password);
                Update Inquiries Set CommentCount = CommentCount + 1 
                Where Id = @BoardId
            ";

            con.Execute(sql, parameters, commandType: CommandType.Text);
        }

        /// <summary>
        /// 특정 게시물에 해당하는 댓글 리스트
        /// </summary>
        public List<InquiryComment> GetInquiryComments(int boardId) => con.Query<InquiryComment>("Select * From InquiryComments Where BoardId = @BoardId", new { BoardId = boardId }, commandType: CommandType.Text).ToList();

        /// <summary>
        /// 특정 게시물의 특정 Id에 해당하는 댓글 카운트
        /// </summary>
        public int GetCountBy(int boardId, int id, string password) => con.Query<int>(@"Select Count(*) From InquiryComments Where BoardId = @BoardId And Id = @Id And Password = @Password", new { BoardId = boardId, Id = id, Password = password }, commandType: CommandType.Text).SingleOrDefault();

        /// <summary>
        /// 댓글 삭제 
        /// </summary>
        public int DeleteInquiryComment(int boardId, int id, string password) => con.Execute(@"Delete InquiryComments Where BoardId = @BoardId And Id = @Id And Password = @Password; Update Inquiries Set CommentCount = CommentCount - 1 Where Id = @BoardId", new { BoardId = boardId, Id = id, Password = password }, commandType: CommandType.Text);
        
        /// <summary>
        /// 최근 댓글 리스트 전체
        /// </summary>
        public List<InquiryComment> GetRecentComments()
        {
            string sql = 
                "SELECT TOP 2 * FROM InquiryComments Order By Id Desc";

            // 캐시에 담을 개체
            List<InquiryComment> cacheData;

            // 캐시에 데이터가 들어있으면 해당 데이터를 가져오기
            if (!_cache.TryGetValue("GetRecentComments", out cacheData))
            {
                // 캐시에 개체 값을 담기
                cacheData = con.Query<InquiryComment>(sql).ToList();

                // 캐시에 현재 시간 저장
                _cache.Set(
                    "GetRecentComments",
                    cacheData,
                    (new MemoryCacheEntryOptions()).SetAbsoluteExpiration(TimeSpan.FromSeconds(60)));
            }

            return cacheData;
        }

        public List<InquiryComment> GetRecentCommentsNoCache() => con.Query<InquiryComment>("SELECT TOP 2 * FROM InquiryComments Order By Id Desc").ToList();
    }
}
