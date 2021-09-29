CREATE TRIGGER [IdSherbimIDyfishte]
	ON [dbo].[Karta]
	FOR INSERT
	AS
	BEGIN
	DECLARE @Id_sh INT;
	SET @Id_sh= (SELECT sherbimId FROM INSERTED);
	UPDATE Karta
	SET Sherbime_Id=@Id_sh
	WHERE Id=(SELECT Id FROM INSERTED)
	END
