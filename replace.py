# -*- coding: utf-8 -*-   

import os  
def getFileList(file_dir):   
    L=[]   
    for dirpath, dirnames, filenames in os.walk(file_dir):  
        for file in filenames :  
            if os.path.splitext(file)[1] == '.cs':  
                L.append(os.path.join(dirpath, file))  
    return L

def replaceDebug():
    files = getFileList("D:\minigame\Project-Trinity\Travel\Assets\Scripts\BaseFunc")
    
    for i in range(len(files)):
        file_name = files[i]
        replace_content = []
        
        with open(file_name, "r") as f:
            whole = f.readlines()
           
            for j in range(len(whole)):
                line = whole[j]
                # print line
                line = line.replace("Debug.Log", "Lucky.LuckyUtils.Log")
                replace_content.append(line)
        
              
        base_name = os.path.basename(file_name)
        saved_file = "D:\minigame\Project-Trinity\Travel\Assets\Scripts\BaseFunc\\" + base_name  
        print saved_file

        with open(saved_file, "w") as f:
            for i in range(len(replace_content)):
                f.write(replace_content[i])


def replaceFont():
    files = getFileList("D:\minigame\Project-Trinity\Travel\Assets\Resources\Prefabs")
    for i in range(len(files)):
        file_name = files[i]
        replace_content = []
        
        with open(file_name, "r") as f:
            whole = f.readlines()
           
            for j in range(len(whole)):
                line = whole[j]
                print line

                if line.strip().startswith("m_Font: "):
                    print line
                # print line
                #line = line.replace("Debug.Log", "Lucky.LuckyUtils.Log")
                replace_content.append(line)
      
        base_name = os.path.basename(file_name)
        saved_file = "D:\minigame\Project-Trinity\Travel\Assets\Scripts\BaseFunc\\" + base_name  
        print saved_file

        # with open(saved_file, "w") as f:
        #     for i in range(len(replace_content)):
        #         f.write(replace_content[i])

if __name__ == "__main__":
    replaceFont()