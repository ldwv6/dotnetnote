--[2] 게시판(DotNetInquiry)에서 데이터 출력: ListInquiries => 업데이트된 InquiriesList 저장 프로시저 사용할 것 
Create Procedure dbo.ListInquiries
    @Page Int
As
    With DotNetInquiryOrderedLists 
    As 
    (
        Select 
            [Id], [Name], [Email], [Title], [PostDate], [ReadCount], 
            [Ref], [Step], [RefOrder], [AnswerNum], [ParentNum], 
            [CommentCount], [FileName], [FileSize], [DownCount], 
            ROW_NUMBER() Over (Order By Ref Desc, RefOrder Asc) 
            As 'RowNumber' 
        From Inquiries
    ) 
    Select * From DotNetInquiryOrderedLists 
    Where RowNumber Between @Page * 10 + 1 And (@Page + 1) * 10
Go
