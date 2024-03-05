using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABTestTracker.Migrations
{
    /// <inheritdoc />
    public partial class CreateStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var spCreateDevice = @$"CREATE PROCEDURE [dbo].[spCreateDevice]
                                 @device_token varchar(100),
                                 @device_id  uniqueidentifier OUTPUT
                                 AS
                                 BEGIN
                                 INSERT INTO [dbo].[devices] (id, device_token)
                                 VALUES (NEWID(), @device_token);


                                 SELECT @device_id = id
                                 FROM [dbo].[devices]
                                 WHERE device_token = @device_token;
                                 END";
            migrationBuilder.Sql(spCreateDevice);

            var spCreatePrice = @$"CREATE PROCEDURE [dbo].[spCreatePrice]

                                @value decimal(5,2),
                                @share decimal(5,2)
                                 AS
                               BEGIN
                               INSERT INTO [dbo].[prices]
                               (id,value,share)
                               VALUES
                               (NEWID(),@value,  @share)
                               END";
            migrationBuilder.Sql(spCreatePrice);


            var spCreateButtonColors = @$"CREATE PROCEDURE [dbo].[spCreateButtonColors]

                                       @value varchar(10),
                                       @share decimal(5,2)
                                       AS
                                       BEGIN
                                       INSERT INTO [dbo].[button_colors]
                                       (id,value,share)
                                       VALUES
                                       (NEWID(),@value, @share)
                                       END";
            migrationBuilder.Sql(spCreateButtonColors);

            var spCreateExperimentButtonColors = @$"CREATE PROCEDURE [dbo].[spCreateExperimentButtonColors]

                                                 @device_id  uniqueidentifier,
                                                 @button_color_id uniqueidentifier,
                                                 @button_color_value VARCHAR(10) OUTPUT
                                                 AS
                                                 BEGIN
                                                 INSERT INTO [dbo].[experiment_button_colors]
                                                 (id,device_id,button_color_id)
                                                 VALUES
                                                 (NEWID(),@device_id,@button_color_id)

                                                 SELECT  @button_color_value = button_colors.value
                                                 FROM [dbo].[button_colors] 
                                                 WHERE button_colors.id =  @button_color_id;
                                                 END";
            migrationBuilder.Sql(spCreateExperimentButtonColors);


            var spCreateExperimentPrices = @$"CREATE PROCEDURE [dbo].[spCreateExperimentPrices]

                                           @device_id  uniqueidentifier,
                                           @price_id uniqueidentifier,
                                           @price decimal(5,2) OUTPUT
                                            AS
                                           BEGIN
                                           INSERT INTO [dbo].[experiment_prices]
                                           (id,device_id,price_id)
                                           VALUES
                                           (NEWID(),@device_id,@price_id)

                                           SELECT @price = prices.value
                                           FROM [dbo].[prices] 
                                           WHERE prices.id =  @price_id;

                                           END";

            migrationBuilder.Sql(spCreateExperimentPrices);

            var spDeviceExistExperimentPrice = @$"CREATE PROCEDURE [dbo].[spDeviceExistExperimentPrice]

                                               @device_token varchar(100),
                                               @Existence INT OUTPUT
                                               AS
                                               BEGIN
                                               IF EXISTS (SELECT 1
                                               FROM experiment_prices AS ep
                                               JOIN devices AS d ON ep.device_id = d.id
                                               WHERE d.device_token = @device_token)
                                               SET @Existence = 1;
                                               ELSE
                                               SET @Existence = 0;
                                               END";

            migrationBuilder.Sql(spDeviceExistExperimentPrice);

            var spDeviceExistExperimentButtonColors = @$"CREATE PROCEDURE [dbo].[spDeviceExistExperimentButtonColors]

                                                     @device_token varchar(100),
											         @Existence INT OUTPUT
                                                     AS
                                                     BEGIN
                                                     IF EXISTS (
                                                     SELECT 1
                                                     FROM experiment_button_colors AS ebc
                                                     INNER JOIN devices AS d ON ebc.device_id = d.id
                                                     WHERE d.device_token = @device_token)
                                                     SET @Existence = 1;
                                                     ELSE
                                                     SET @Existence = 0;
                                                     END";

            migrationBuilder.Sql(spDeviceExistExperimentButtonColors);


            var spFindDeviceByToken = @$"CREATE PROCEDURE [dbo].[spFindDeviceByToken]

                                               @device_token varchar(100)
                                               AS
                                               BEGIN
                                               SELECT * FROM devices
                                               WHERE device_token=@device_token
                                               END";

            migrationBuilder.Sql(spFindDeviceByToken);


            var spFindDevicesByButtonColors = @$"CREATE PROCEDURE [dbo].[spFindDevicesByButtonColors]

                                               @value varchar(10)
                                               AS
                                               BEGIN
                                               SELECT devices.*
                                               FROM devices
                                               JOIN experiment_button_colors ON devices.id = experiment_button_colors.device_id
                                               JOIN button_colors ON experiment_button_colors.button_color_id = button_colors.id
                                               WHERE button_colors.value =  @value;
                                               END";

            migrationBuilder.Sql(spFindDevicesByButtonColors);


            var spFindDevicesByPrice = @$"CREATE PROCEDURE [dbo].[spFindDevicesByPrice]

                                               @share decimal(5,2)
                                               AS
                                               BEGIN
                                               SELECT devices.*
                                               FROM devices
                                               JOIN experiment_prices ON devices.id = experiment_prices.device_id
                                               JOIN prices ON experiment_prices.price_id = prices.id
                                               WHERE prices.share = @share;
                                               END";

            migrationBuilder.Sql(spFindDevicesByPrice);

            var spFindButtonColorByToken = @$"CREATE PROCEDURE [dbo].[spFindButtonColorByToken]

                                              @device_token VARCHAR(100),
                                              @button_color_value VARCHAR(10) OUTPUT
                                               AS
                                              BEGIN
                                              SELECT @button_color_value =button_colors.value
                                              FROM devices
                                              JOIN experiment_button_colors  ON devices.id = experiment_button_colors.device_id
                                              JOIN button_colors  ON experiment_button_colors.button_color_id = button_colors.id
                                              WHERE devices.device_token = @device_token;
                                               END";
            migrationBuilder.Sql(spFindButtonColorByToken);

            var spFindPriceByToken = @$"CREATE PROCEDURE [dbo].[spFindPriceByToken]

                                              @device_token VARCHAR(100),
                                              @price decimal(5,2) OUTPUT
                                               AS
                                              BEGIN
                                              SELECT @price = prices.value
                                              FROM devices
                                              JOIN experiment_prices  ON devices.id = experiment_prices.device_id
                                              JOIN price  ON experiment_prices.price_id = price.id
                                              WHERE devices.device_token = @device_token;
                                               END";
            migrationBuilder.Sql(spFindPriceByToken);

            var spGetAmountByButtonColor = @$"CREATE PROCEDURE [dbo].[spGetAmountByButtonColor]

                                               @value varchar(10),
                                               @amount INT OUTPUT 
                                               AS
                                               BEGIN
                                               SELECT  @amount = COUNT(devices.id) 
                                               FROM devices
                                               JOIN experiment_button_colors ON devices.id = experiment_button_colors.device_id
                                               JOIN button_colors ON experiment_button_colors.button_color_id = button_colors.id
                                               WHERE button_colors.value = @value
                                               END";

            migrationBuilder.Sql(spGetAmountByButtonColor);

            var spGetTotalAmountButtonColorExperiment = @$"CREATE PROCEDURE [dbo].[spGetTotalAmountButtonColorExperiment]
                                              @amount INT OUTPUT 
                                               AS
                                               BEGIN
                                               SELECT  @amount = COUNT(device_id) 
                                               FROM experiment_button_colors
                                               GROUP BY device_id
                                               END";

            migrationBuilder.Sql(spGetTotalAmountButtonColorExperiment);

            var spGetTotalAmountPriceExperiment = @$"CREATE PROCEDURE [dbo].[spGetTotalAmountPriceExperiment]
                                              @amount INT OUTPUT 
                                               AS
                                               BEGIN
                                               SELECT  @amount = COUNT(device_id) 
                                               FROM experiment_prices
                                               GROUP BY device_id
                                               END";

            migrationBuilder.Sql(spGetTotalAmountPriceExperiment);

            var spGetAmountByPrice = @$"CREATE PROCEDURE [dbo].[spGetAmountByPrice]

                                               @value decimal(5,2),
                                               @amount INT OUTPUT 
                                               AS
                                               BEGIN
                                               SELECT  @amount = COUNT(devices.id) 
                                               FROM devices
                                               JOIN experiment_prices ON devices.id = experiment_prices.device_id
                                               JOIN prices ON experiment_prices.price_id = prices.id
                                               WHERE prices.value =  @value;
                                               END";

            migrationBuilder.Sql(spGetAmountByPrice);


            var spRemoveDeviceFromBtnColorExperiment = @$"CREATE PROCEDURE [dbo].[spRemoveDeviceFromBtnColorExperiment]

                                               @device_id  uniqueidentifier
                                               AS
                                               BEGIN
                                               DELETE FROM experiment_button_colors
                                               WHERE device_id = @device_id;
                                               END";

            migrationBuilder.Sql(spRemoveDeviceFromBtnColorExperiment);

            var spRemoveDeviceFromPriceExperiment = @$"CREATE PROCEDURE [dbo].[spRemoveDeviceFromPriceExperiment]

                                               @device_id  uniqueidentifier
                                               AS
                                               BEGIN
                                               DELETE FROM experiment_prices
                                               WHERE device_id = @device_id;
                                               END";

            migrationBuilder.Sql(spRemoveDeviceFromPriceExperiment);

            var spRemoveDevice = @$"CREATE PROCEDURE [dbo].[spRemoveDevice]

                                               @device_id  uniqueidentifier
                                               AS
                                               BEGIN
                                               DELETE FROM devices
                                               WHERE id = @device_id;
                                               END";

            migrationBuilder.Sql(spRemoveDevice);

            var spGetListOfPrices = @$"CREATE PROCEDURE [dbo].[spGetListOfPrices]
                                               AS
                                               BEGIN
                                               SELECT * FROM prices
                                               END";

            migrationBuilder.Sql(spGetListOfPrices);

            var spGetListOfButtonColors = @$"CREATE PROCEDURE [dbo].[spGetListOfButtonColors]
                                               AS
                                               BEGIN
                                               SELECT * FROM button_colors
                                               END";

            migrationBuilder.Sql(spGetListOfButtonColors);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropProcCreateDevice = "DROP PROC spCreateDevice";
            migrationBuilder.Sql(dropProcCreateDevice);

            var dropProcCreatePrice = "DROP PROC spCreatePrice";
            migrationBuilder.Sql(dropProcCreatePrice);

            var dropProcCreateButtonColors = "DROP PROC spCreateButtonColors";
            migrationBuilder.Sql(dropProcCreateButtonColors);

            var dropProcCreateExperimentButtonColors = "DROP PROC spCreateExperimentButtonColors";
            migrationBuilder.Sql(dropProcCreateExperimentButtonColors);

            var dropProcCreateExperimentPrices = "DROP PROC spCreateExperimentPrices";
            migrationBuilder.Sql(dropProcCreateExperimentPrices);

            var dropProcDeviceExistExperimentPrice = "DROP PROC spDeviceExistExperimentPrice";
            migrationBuilder.Sql(dropProcDeviceExistExperimentPrice);

            var dropProcDeviceExistExperimentButtonColors = "DROP PROC spDeviceExistExperimentButtonColors";
            migrationBuilder.Sql(dropProcDeviceExistExperimentButtonColors);

            var dropProcFindDeviceByToken = "DROP PROC spFindDeviceByToken";
            migrationBuilder.Sql(dropProcFindDeviceByToken);

            var dropProcFindDevicesByButtonColors = "DROP PROC spFindDeviceByToken";
            migrationBuilder.Sql(dropProcFindDevicesByButtonColors);

            var dropProcFindDevicesByPrice = "DROP PROC spFindDevicesByPrice";
            migrationBuilder.Sql(dropProcFindDevicesByPrice);

            var dropProcGetAmountByButtonColor = "DROP PROC spGetAmountByButtonColor";
            migrationBuilder.Sql(dropProcGetAmountByButtonColor);

            var dropProcGetAmountByPrice = "DROP PROC spGetAmountByPrice";
            migrationBuilder.Sql(dropProcGetAmountByPrice);

            var dropProcRemoveDeviceFromBtnColorExperiment = "DROP PROC spRemoveDeviceFromBtnColorExperiment";
            migrationBuilder.Sql(dropProcRemoveDeviceFromBtnColorExperiment);

            var dropProcspRemoveDeviceFromPriceExperiment = "DROP PROC spRemoveDeviceFromPriceExperiment";
            migrationBuilder.Sql(dropProcspRemoveDeviceFromPriceExperiment);

            var dropProcRemoveDevice = "DROP PROC spRemoveDevice";
            migrationBuilder.Sql(dropProcRemoveDevice);

            var dropProcFindButtonColorByToken = "ROP POC spFindButtonColorByToken";
            migrationBuilder.Sql(dropProcFindButtonColorByToken);

            var dropProcFindPriceByToken = "DROP PROC spFindPriceByToken";
            migrationBuilder.Sql(dropProcFindPriceByToken);

            var dropProcGetListOfPrices = "DROP PROC spGetListOfPrices";
            migrationBuilder.Sql(dropProcGetListOfPrices);

            var dropProcGetListOfButtonColors = "DROP PROC spGetListOfButtonColors";
            migrationBuilder.Sql(dropProcGetListOfButtonColors);

        }
    }
}
