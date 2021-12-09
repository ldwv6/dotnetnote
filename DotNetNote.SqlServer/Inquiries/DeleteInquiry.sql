﻿--[7] 해당 글을 지우는 저장 프로시저(답변글이 있으면 업데이트하고 없으면 지운다.): DeleteInquiry, InquiriesDelete
Create Proc dbo.DeleteInquiry
    @Id Int,				-- 일련번호 
    @Password NVarChar(255) -- 암호 매개변수
As
    --[A] 등록한 암호가 맞는지 확인
    Declare @cnt Int
    Select @cnt = Count(*) From Inquiries
    Where Id = @Id And Password = @Password

    If @cnt = 0
    Begin
        Return 0 -- 번호와 암호가 맞는게 없으면 0을 반환
    End	

	--[B] 등록한 암호가 맞으면 삭제 진행
    Declare @Ref Int
    Declare @RefOrder Int
    Declare @AnswerNum Int
    Declare @ParentNum Int

	--[a] 삭제하려는 글의 정보 읽어오기 
    Select
        @AnswerNum = AnswerNum,     @RefOrder = RefOrder,
        @Ref = Ref,                 @ParentNum = ParentNum
    From Inquiries
    Where Id = @Id

    If @AnswerNum = 0
    Begin
		--[b] 답변 글이 없으면 바로 삭제
        If @RefOrder > 0 
        Begin
            UPDATE Inquiries SET RefOrder = RefOrder - 1
            WHERE Ref = @Ref AND RefOrder > @RefOrder
            UPDATE Inquiries SET AnswerNum = AnswerNum - 1 WHERE Id = @ParentNum
        End
        Delete Inquiries Where Id = @Id
        Delete Inquiries 
        WHERE 
            Id = @ParentNum AND ModifyIp = N'((DELETED))' AND AnswerNum = 0	
    End
    Else
    Begin
        --[c] 답변 글이 있으면 삭제하지 않고 내용만 Null 값으로 업데이트
        Update Inquiries 
        Set 
            Name = N'(Unknown)', Email = '', Password = '',
            Title = N'(삭제된 글입니다.)', 
            Content = N'(삭제된 글입니다. ' 
            + N'현재 답변이 포함되어 있기 때문에 내용만 삭제되었습니다.)',
            ModifyIp = N'((DELETED))', FileName = '', 
            FileSize = 0, CommentCount = 0
        Where Id = @Id  
    End
Go