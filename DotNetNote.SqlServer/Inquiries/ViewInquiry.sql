--[3] 해당 글을 세부적으로 읽어오는 저장 프로시저: ViewInquiry, InquiriesView
Create Procedure dbo.ViewInquiry
    @Id Int
As
    -- 조회수 카운트 1증가
    Update Inquiries Set ReadCount = ReadCount + 1 Where Id = @Id
    
    -- 모든 항목 조회
    Select * From Inquiries Where Id = @Id
Go
