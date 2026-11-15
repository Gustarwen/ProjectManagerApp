package com.gustaw_project_manager
import org.jetbrains.exposed.dao.IntEntity
import org.jetbrains.exposed.dao.IntEntityClass
import org.jetbrains.exposed.dao.id.EntityID
import org.jetbrains.exposed.dao.id.IntIdTable
import org.jetbrains.exposed.sql.javatime.date;

class Time {
    object TimeTable : IntIdTable("CodingTime") {
        val date = date("date");

        val time = double("time")
    }
}
class TimeEntity(id:EntityID<Int>) : IntEntity(id) {
    companion object : IntEntityClass<TimeEntity>(Time.TimeTable)
    var date by Time.TimeTable.date
    var time by Time.TimeTable.time;
}