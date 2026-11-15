package com.gustaw_project_manager

import com.intellij.codeInsight.editorActions.TypedHandlerDelegate
import com.intellij.notification.NotificationGroupManager
import com.intellij.notification.NotificationType
import com.intellij.openapi.application.ApplicationManager
import com.intellij.openapi.editor.Editor
import com.intellij.openapi.project.Project
import com.intellij.psi.PsiFile
import com.jetbrains.rd.generator.nova.PredefinedType
import org.jetbrains.exposed.sql.transactions.transaction
import java.time.LocalTime
import org.jetbrains.exposed.sql.SqlExpressionBuilder.eq
import java.time.LocalDate
import java.time.LocalDateTime
import java.util.concurrent.atomic.AtomicLong

val lastActionTime = AtomicLong(System.currentTimeMillis())
val acumulatedMs = AtomicLong(0L);

private const val MaxToReject = 2*60*1000L
private const val MinToAccept = 1*1000L

class MyTimeChacker : TypedHandlerDelegate() {

    fun recordUserTime(){
        val now = System.currentTimeMillis()

        val previous = lastActionTime.getAndSet(now)
        val diffMs = now - previous
        if(diffMs in 1..MaxToReject){
            val totalTimeToSave = acumulatedMs.getAndAdd(diffMs);
            if(totalTimeToSave>MinToAccept){
                val ms = acumulatedMs.getAndSet(0L);
                var SecondsToAdd =ms.toDouble()/1000.0;
                saveTimeToDb(SecondsToAdd)
                println("ahoj");

            }
        }

    }
    fun saveTimeToDb(secondstoAdd:Double)
    {
        val now = LocalDate.now()
        ApplicationManager.getApplication().executeOnPooledThread {
            val existingRecord = TimeEntity.find{
                Time.TimeTable.date eq now
            }.firstOrNull()
            if (existingRecord != null) {
                existingRecord.time += secondstoAdd;
            }else{
                TimeEntity.new {
                    date=now
                    time = secondstoAdd
                }
            }

        }
    }
    // DŮLEŽITÉ: Tady NESMÍ být slovo "suspend"
    override fun charTyped(c: Char, project: Project, editor: Editor, file: PsiFile): Result {
        recordUserTime()

        

        // DŮLEŽITÉ: Tenhle řádek tam musí být na konci
        return Result.CONTINUE
    }
}