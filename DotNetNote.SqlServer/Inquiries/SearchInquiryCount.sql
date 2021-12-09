--[6] 검색 결과의 레코드 수 반환: SearchInquiryCount, InquiriesSearchCount 
Create Proc dbo.SearchInquiryCount
    @SearchField NVarChar(25),
    @SearchQuery NVarChar(25)
As
    Set @SearchQuery = '%' + @SearchQuery + '%'

    Select Count(*)
    From Inquiries
    Where
    (
        Case @SearchField 
            When 'Name' Then [Name]
            When 'Title' Then Title
            When 'Content' Then Content
            Else @SearchQuery
        End
    ) 
    Like 
    @SearchQuery
Go
