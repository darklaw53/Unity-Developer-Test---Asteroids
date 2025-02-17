def PROJECT_NAME = "Unity-Developer-Test---Asteroids"
def CUSTOM_WORKSPACE = "C:\\Users\\Osten Games\\Documents\\GitHub\\Unity-Developer-Test---Asteroids\\${PROJECT_NAME}"
def UNITY_VERSION = "2022.3.42f1"
def UNITY_INSTALLATION = "D:\\unity\\${UNITY_VERSION}\\Editor"

pipeline {
    environment {
        PROJECT_PATH = "${CUSTOM_WORKSPACE}\\${PROJECT_NAME}"
        UNITY_LICENSE_PATH = "C:\\ProgramData\\Jenkins\\Unity\\Unity_lic.ulf"
    }

    agent {
        label {
            label ""
            customWorkspace "${CUSTOM_WORKSPACE}"
        }
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Activate Unity License') {
            steps {
                script {
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]) {
                        bat '''
                        if not exist "%UNITY_LICENSE_PATH%" (
                            echo "Generating manual activation file..."
                            "%UNITY_PATH%/Unity.exe" -batchmode -createManualActivationFile -logFile -quit
                            echo "Upload the generated .alf file to Unity's license page and download the .ulf file."
                            exit /b 1
                        ) else (
                            echo "Applying Unity license..."
                            "%UNITY_PATH%/Unity.exe" -batchmode -manualLicenseFile "%UNITY_LICENSE_PATH%" -logFile -quit
                        )
                        '''
                    }
                }
            }
        }

        stage('Build Windows') {
            when {
                expression { BUILD_WINDOWS == 'true' }
            }
            steps {
                script {
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]) {
                        bat '''
                        "%UNITY_PATH%/Unity.exe" -quit -batchmode -projectPath %PROJECT_PATH% -executeMethod BuildScript.BuildWindows -logFile -
                        '''
                    }
                }
            }
        }

        stage('Deploy Windows') {
            when {
                expression { DEPLOY_WINDOWS == 'true' }
            }
            steps {
                echo 'Deploy Windows'
            }
        }
    }

    post {
        always {
            cleanWs()
        }
    }
}