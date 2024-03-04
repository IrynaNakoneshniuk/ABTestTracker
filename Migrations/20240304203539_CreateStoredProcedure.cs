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

                                 @device_token varchar(100)
                                 AS
                                 BEGIN
                                 INSERT INTO [dbo].[devices]
                                 (id ,device_token)
                                 VALUES
                                 (NEWID(),@device_token)
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
                                                 @button_color_id uniqueidentifier
                                                 AS
                                                 BEGIN
                                                 INSERT INTO [dbo].[experiment_button_colors]
                                                 (id,device_id,button_color_id)
                                                 VALUES
                                                 (NEWID(),@device_id,@button_color_id)
                                                 END";
            migrationBuilder.Sql(spCreateExperimentButtonColors);


            var spCreateExperimentPrices = @$"CREATE PROCEDURE [dbo].[spCreateExperimentPrices]

                                           @device_id  uniqueidentifier,
                                           @price_id uniqueidentifier
                                            AS
                                           BEGIN
                                           INSERT INTO [dbo].[experiment_prices]
                                           (id,device_id,price_id)
                                           VALUES
                                           (NEWID(),@device_id,@price_id)
                                           END";

            migrationBuilder.Sql(spCreateExperimentPrices);

            var spDeviceExistExperimentPrice = @$"CREATE PROCEDURE [dbo].[spDeviceExistExperimentPrice]

                                               @device_id  uniqueidentifier,
                                               AS
                                               BEGIN
                                               IF EXISTS (SELECT * FROM experiment_prices WHERE device_id = @deviceId)
                                               SELECT 1;
                                               ELSE
                                               SELECT 0; 
                                               END";

            migrationBuilder.Sql(spDeviceExistExperimentPrice);

            var spDeviceExistExperimentButtonColors = @$"CREATE PROCEDURE [dbo].[spDeviceExistExperimentButtonColors]

                                               @device_id  uniqueidentifier,
                                               AS
                                               BEGIN
                                               IF EXISTS (SELECT * FROM experiment_button_colors WHERE device_id = @deviceId)
                                               SELECT 1;
                                               ELSE
                                               SELECT 0; 
                                               END";

            migrationBuilder.Sql(spDeviceExistExperimentButtonColors);


            var spFindDeviceByToken = @$"CREATE PROCEDURE [dbo].[spDeviceExistExperimentButtonColors]

                                               @device_token varchar(100)
                                               AS
                                               BEGIN
                                               SELECT * FROM devices
                                               WHERE device_token=  @device_token
                                               END";

            migrationBuilder.Sql(spFindDeviceByToken);


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
        }
    }
}
