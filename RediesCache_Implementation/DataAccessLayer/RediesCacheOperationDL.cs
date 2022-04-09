using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using RediesCache_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RediesCache_Implementation.DataAccessLayer
{
    public class RediesCacheOperationDL : IRediesCacheOperationDL
    {
        public readonly IConfiguration _configuration;
        public readonly MySqlConnection _mySqlConnection;
        public RediesCacheOperationDL(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new MySqlConnection(_configuration["ConnectionStrings:MySqlDBConnectionString"]);
        }

        public async Task<AddInformationResponse> AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"INSERT INTO crudoperation.crudapplication
                                    (UserName, EmailID, MobileNumber, Salary, Gender) VALUES
                                    (@UserName, @EmailID, @MobileNumber, @Salary, @Gender);";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@EmailID", request.EmailID);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", request.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@Salary", request.Salary);
                    sqlCommand.Parameters.AddWithValue("@Gender", request.Gender);
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "AddInformation Query Not Exxecuted";
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<DeleteInformationResponse> DeleteInformation(DeleteInformationRequest request)
        {
            DeleteInformationResponse response = new DeleteInformationResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"DELETE
                                    FROM crudoperation.crudapplication
                                    WHERE UserId=@UserID";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserID", request.UserID);
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = $"{request.UserID} Not Found";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetInformationResponse> GetInformation(GetInformationRequest request)
        {
            GetInformationResponse response = new GetInformationResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT *
                                    FROM crudoperation.crudapplication
                                    WHERE UserId=@UserId";


                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@UserId", request.UserID);
                    sqlCommand.CommandTimeout = 180;
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            response.data = new GetInformation();

                            if (await dataReader.ReadAsync())
                            {
                                response.data.UserID = dataReader["UserId"] != DBNull.Value ? Convert.ToInt32(dataReader["UserId"]) : -1;
                                response.data.UserName = dataReader["UserName"] != DBNull.Value ? dataReader["UserName"].ToString() : string.Empty;
                                response.data.EmailID = dataReader["EmailID"] != DBNull.Value ? dataReader["EmailID"].ToString() : string.Empty;
                                response.data.MobileNumber = dataReader["MobileNumber"] != DBNull.Value ? dataReader["MobileNumber"].ToString() : string.Empty;
                                response.data.Salary = dataReader["Salary"] != DBNull.Value ? Convert.ToInt32(dataReader["Salary"]) : -1;
                                response.data.Gender = dataReader["Gender"] != DBNull.Value ? dataReader["Gender"].ToString() : string.Empty;

                            }
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Record Not Found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<RefreshRecordTimeResponse> RefreshRecordTime()
        {
            RefreshRecordTimeResponse response = new RefreshRecordTimeResponse();
            response.IsSuccess = true;
            response.Message = "Successful";

            try
            {

                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT UserId FROM crudoperation.crudapplication";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            response.data = new List<RefreshRecordTime>();
                            while (await dataReader.ReadAsync())
                            {
                                response.data.Add(new RefreshRecordTime()
                                {
                                    UserId = dataReader["UserId"] != DBNull.Value ? Convert.ToInt32(dataReader["UserId"]) : -1
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<UpdateInformationResponse> UpdateInformation(UpdateInformationRequest request)
        {
            UpdateInformationResponse response = new UpdateInformationResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"UPDATE crudoperation.crudapplication
                                    SET UserName=@UserName, 
                                        EmailID=@EmailID, 
                                        MobileNumber=@MobileNumber, 
                                        Salary=@Salary, 
                                        Gender=@Gender
                                    WHERE UserId=@UserID";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;

                    sqlCommand.Parameters.AddWithValue("@UserID", request.UserID);
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@EmailID", request.EmailID);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", request.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@Salary", request.Salary);
                    sqlCommand.Parameters.AddWithValue("@Gender", request.Gender);

                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = $"{request.UserID} Not Found";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }
    }
}
