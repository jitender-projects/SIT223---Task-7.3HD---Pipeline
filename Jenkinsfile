pipeline {
    agent any

    stages {
        stage('1. Build') {
            steps {
                echo 'Starting the Build Stage...'
                // Restores dependencies and builds the .NET project using Windows batch commands
                bat 'dotnet restore'
                bat 'dotnet build --configuration Release --no-restore'
            }
        }
    }
}