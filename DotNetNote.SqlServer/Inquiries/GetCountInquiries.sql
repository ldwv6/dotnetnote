--[5] DotNetInquiry 테이블에 있는 레코드의 개수를 구하는 저장 프로시저: GetCountInquiries, InquiriesCount
Create Proc dbo.GetCountInquiries
As
    Select Count(*) From Inquiries
Go
