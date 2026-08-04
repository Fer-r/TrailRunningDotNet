# Makefile for Racehub (ASP.NET Core 10 + React Vite)

# Variables
BACKEND_DIR := src/RacehubApi
FRONTEND_DIR := src/RacehubWeb

.PHONY: help init install up backend frontend build db migration test clean

help: ## Display this help message
	@echo "Usage: make [target]"
	@echo ""
	@echo "Available targets:"
	@echo "  init        Install dependencies, apply database migrations"
	@echo "  install     Install backend (NuGet) and frontend (npm) dependencies"
	@echo "  up          Start both backend and frontend development servers concurrently"
	@echo "  backend     Start ONLY the .NET backend API"
	@echo "  frontend    Start ONLY the React Vite frontend"
	@echo "  build       Build both backend and frontend for production"
	@echo "  db          Run EF Core database update (apply migrations to SQLite)"
	@echo "  migration   Add a new EF Core migration (usage: make migration NAME=AddUsers)"
	@echo "  test        Run backend xUnit tests"
	@echo "  clean       Clean .NET build artifacts and frontend node_modules"

init: install db ## Install dependencies and setup database
	@echo "Project initialized successfully!"

install: ## Install backend and frontend dependencies
	@echo "Restoring .NET packages..."
	cd $(BACKEND_DIR) && dotnet restore
	@echo "Installing npm packages..."
	cd $(FRONTEND_DIR) && npm install

up: ## Start both backend and frontend concurrently (requires bash/concurrently)
	@echo "Starting Backend and Frontend..."
	# Using npm concurrently or background processes
	cd $(FRONTEND_DIR) && npm run dev & cd $(BACKEND_DIR) && dotnet run

backend: ## Start .NET backend
	@echo "Starting .NET API on http://localhost:5000..."
	cd $(BACKEND_DIR) && dotnet run

frontend: ## Start React frontend
	@echo "Starting React Dev Server..."
	cd $(FRONTEND_DIR) && npm run dev

build: ## Build both projects
	@echo "Building .NET backend..."
	cd $(BACKEND_DIR) && dotnet build --configuration Release
	@echo "Building React frontend..."
	cd $(FRONTEND_DIR) && npm run build

db: ## Apply EF Core migrations to SQLite database
	@echo "Applying Entity Framework Core migrations..."
	cd $(BACKEND_DIR) && dotnet ef database update

migration: ## Add a new migration (Example: make migration NAME=Init)
	@if [ -z "$(NAME)" ]; then \
		echo "Error: Migration NAME is not set. Use: make migration NAME=YourMigrationName"; \
	else \
		echo "Creating EF Core migration: $(NAME)..."; \
		cd $(BACKEND_DIR) && dotnet ef migrations add $(NAME); \
	fi

test: ## Run xUnit tests
	@echo "Running .NET tests..."
	cd $(BACKEND_DIR) && dotnet test

clean: ## Clean build artifacts
	@echo "Cleaning .NET bin/obj folders..."
	cd $(BACKEND_DIR) && dotnet clean
	@echo "Removing frontend node_modules..."
	rm -rf $(FRONTEND_DIR)/node_modules
	rm -rf $(FRONTEND_DIR)/dist