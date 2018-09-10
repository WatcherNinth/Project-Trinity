# -*- coding: utf-8 -*-
import sqlite3
import xlrd
import sys
from xlrd import xldate_as_tuple

from datetime import date, datetime
import datetime
import time
import sqlite3

reload(sys)
sys.setdefaultencoding('utf-8')

init_time = datetime.datetime.strptime("1970-01-01 8:00:00", "%Y-%m-%d %H:%M:%S")
# print init_time
ts = time.mktime(init_time.timetuple())

def InsertData(t):
    sql = "insert into routine(start_node, end_node, start_time, end_time, type, money, ticket_name) values(\""  + \
    t.start_node + "\",\"" + t.end_node + "\"," + str(t.start_time) + "," + str(t.end_time) + ", " + str(0)  + ", " + str(t.expense) + ","  + "\"" \
    + t.tickets_name +"\")"
    print sql

    conn = sqlite3.connect('Travel.db')
    c = conn.cursor()

    c.execute(sql)
    conn.commit()
    conn.close()

class Tickets:
    def __init__(self, start_node, end_node, tickets_name, start_time, end_time, expense):
        self.start_node = start_node
        self.end_node = end_node
        self.tickets_name = tickets_name
        self.start_time = start_time
        self.end_time = end_time
        self.expense = expense

    @property
    def start_node(self):
        return self.start_node

    @property
    def end_node(self):
        return self.end_node

    @property
    def tickets_sname(self):
        return self.tickets_name

    @property
    def start_time(self):
        return self.start_time

    @property
    def end_time(self):
        return self.end_time

    @property
    def expense(self):
        return self.expense

def read_excel():
    # 文件位置
    ExcelFile = xlrd.open_workbook(r'D:\minigame\ImportTravleData\rail_transport.xlsx')
    sheet_names_list = ExcelFile.sheet_names()
    
    ticket_list = list()

    for i in range(len(sheet_names_list)):
        if i == 0 or i == 1:
            # 读取每张列表
            sheet = ExcelFile.sheet_by_name(sheet_names_list[i])
            nrows = sheet.nrows
        
            for j in range(nrows):
                if j == 0:
                    continue

                start_node = sheet.row_values(j)[0]

                end_node = sheet.row_values(j)[1]

                tickets_name = "".join(str(sheet.row_values(j)[2]).split());
                
                if sheet.cell(j, 3).ctype == 3:
                    cell = sheet.cell_value(j, 3)
                    start_time = xldate_as_tuple(cell, 0)
                else:
                    print ", is str start time "
                # print init_time + datetime.timedelta(hours=1)
                new_time = init_time + datetime.timedelta(hours = start_time[3], minutes = start_time[4])
            
                start_time_int = int(time.mktime(new_time.timetuple()))
                
                if sheet.cell(j, 4).ctype == 3:
                    cell = sheet.cell_value(j, 4)
                    end_time  = xldate_as_tuple(cell, 0)
                else:
                    print ", is str start time"
                
                end_time_int = 0
                if end_time[3] < start_time[3]:
                    end_time_int  += end_time[3] + 12
                
                new_time = init_time + datetime.timedelta(hours = end_time_int, minutes = end_time[4])
                end_time_int = int(time.mktime(new_time.timetuple()))

                expense = str(sheet.row_values(j)[5])
                print start_node, end_node, tickets_name, start_time_int, end_time_int, expense
                
                t = Tickets(start_node, end_node, tickets_name, start_time_int, end_time_int, expense)
                InsertData(t)


            
        if i >= 2:
            # 读取每张列表
            sheet = ExcelFile.sheet_by_name(sheet_names_list[i])
            nrows = sheet.nrows   
        
            for j in range(nrows):
                if j == 0:
                    continue

                tickets_name = str(sheet.row_values(j)[0]).encode("utf-8").decode("gbk")

                start_node = sheet.row_values(j)[1]

                end_node = sheet.row_values(j)[2]

                if sheet.cell(j, 3).ctype == 3:
                    cell = sheet.cell_value(j, 3)
                    start_time = xldate_as_tuple(cell, 0)
                    new_time = init_time + datetime.timedelta(hours = start_time[3], minutes = start_time[4])
                    start_time_int = int(time.mktime(new_time.timetuple()))
                else:
                    start_time = sheet.row_values(j)[3]
                    start_time_split_result = start_time.split(' ')
                    print tickets_name, " split, end time ", start_time_split_result[1]
                    hour = int(start_time_split_result[1].split(":")[0]) + 12
                    minute = int(start_time_split_result[1].split(":")[1])
                    new_time = init_time + datetime.timedelta(hours = hour, minutes = minute)
                    start_time_int = int(time.mktime(new_time.timetuple()))
                    print start_time_int

                # print "start_time ", start_time

                if sheet.cell(j, 4).ctype == 3:
                    cell = sheet.cell_value(j, 4)
                    end_time = xldate_as_tuple(cell, 0)
                    hour = end_time[3]

                    if end_time[3] < start_time[3]:
                        hour += 12

                    new_end_time = init_time + datetime.timedelta(hours = hour, minutes = end_time[4])
                    end_time_int = int(time.mktime(new_end_time.timetuple()))
                else:
                    end_time = sheet.row_values(j)[4]
                    end_time_split_result = end_time.split(' ')
                    print tickets_name, " split, end time ", end_time_split_result[1]
                    hour = int(end_time_split_result[1].split(":")[0]) + 12
                    minute = int(end_time_split_result[1].split(":")[1])
                    new_end_time = init_time + datetime.timedelta(hours = hour, minutes = minute)
                    end_time_int = int(time.mktime(new_end_time.timetuple()))
                    print end_time_int

                #end_time = sheet.row_values(j)[4]
                # print end_time

                expense = sheet.row_values(j)[5]

                # expense = str(sheet.row_values(j)[5])
                print start_node, end_node, tickets_name, start_time_int, end_time_int , expense
                
                t = Tickets(start_node, end_node, tickets_name, start_time_int, end_time_int, expense)
                InsertData(t)

def main():
    read_excel()

if __name__ == "__main__":
    main()