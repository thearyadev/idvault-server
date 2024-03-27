run:
	dotnet run --urls http://0.0.0.0:3000

update:
	dotnet ef database update
migrate:
	dotnet ef migrations add $(ARGS)
format: 
	dotnet format
	find . -type f -name "*.cs" -exec sed -i 's/\r//g' {} +

