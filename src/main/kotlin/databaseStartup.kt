package com.gustaw_project_manager

import com.intellij.openapi.project.Project
import com.intellij.openapi.startup.ProjectActivity
import org.jetbrains.exposed.sql.Database
class databaseStartup : ProjectActivity {
    override suspend fun execute(project: Project) {
        DataBaseManager.init();
    }
}