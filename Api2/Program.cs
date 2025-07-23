using Api1.Grpc.Protos;
using Api2.GrpcServcies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("products", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ProductsBaseUrl")!);
});

builder.Services.AddScoped<ProductGrpcService>();
builder.Services.AddGrpcClient<Api1GrpcService.Api1GrpcServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration.GetValue<string>("ProductsBaseUrl")!);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
