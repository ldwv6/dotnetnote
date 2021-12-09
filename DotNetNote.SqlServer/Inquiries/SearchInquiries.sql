--[9] 게시판(DotNetInquiry)에서 데이터 검색 리스트: SearchInquiries, InquiriesSearchList => InquiriesSearchList 저장 프로시저로 대체할 것
Create Procedure dbo.SearchInquiries
    @Page Int,
    @SearchField NVarChar(25),
    @SearchQuery NVarChar(25)
As
    With DotNetInquiryOrderedLists 
    As 
    (
        Select 
            [Id], [Name], [Email], [Title], [PostDate], 
            [ReadCount], [Ref], [Step], [RefOrder], [AnswerNum], 
            [ParentNum], [CommentCount], [FileName], [FileSize], 
            [DownCount], 
            ROW_NUMBER() Over (Order By Ref Desc, RefOrder Asc) 
            As 'RowNumber' 
        From Inquiries
        Where ( 
            Case @SearchField 
                When 'Name' Then [Name] 
                When 'Title' Then Title 
                When 'Content' Then Content 
                Else 
                @SearchQuery 
            End 
        ) Like '%' + @SearchQuery + '%'
    ) 
    Select 
        [Id], [Name], [Email], [Title], [PostDate], 
        [ReadCount], [Ref], [Step], [RefOrder], 
        [AnswerNum], [ParentNum], [CommentCount], 
        [FileName], [FileSize], [DownCount], 
        [RowNumber] 
    From DotNetInquiryOrderedLists 
    Where RowNumber Between @Page * 10 + 1 And (@Page + 1) * 10  
    Order By Id Desc 
Go
