package com.gustaw_project_manager
import com.intellij.openapi.application.PathManager
import org.jetbrains.exposed.sql.Database
import org.jetbrains.exposed.sql.SchemaUtils
import org.jetbrains.exposed.sql.transactions.transaction
import java.io.File
object DataBaseManager {
    public fun init(){
        val dbFile = File(PathManager.getSystemPath(),"my-rider-plugin.db")
        Class.forName("org.sqlite.JDBC")
        Database.connect("jdbc:sqlite:${dbFile.absolutePath}", driver = "org.sqlite.JDBC")
        transaction {
            SchemaUtils.create(Time.TimeTable)
        }
        println("DATABAZE USPESNE VYTVORENA!")

    }
}