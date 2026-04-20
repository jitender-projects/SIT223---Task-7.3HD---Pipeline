pipeline {
    agent any

    stages {
        stage('1. Build') {
            steps {
                echo 'Starting the Build Stage...'
                bat 'dotnet restore'
                bat 'dotnet build --configuration Release --no-restore'
            }
        }
        stage('2. Test') {
            steps {
                echo 'Running automated tests...'
                // The --no-build flag saves time by using the code we just compiled in Stage 1
                bat 'dotnet test --configuration Release --no-build --verbosity normal'
            }
        }
    }
}
