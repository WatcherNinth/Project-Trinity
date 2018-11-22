#！/bin/bash

adb devices

de=()
i=0;

IFS=$'\n'
for line in $(adb devices)
do
	
	if [ $i -ne 0 ]; then
		device=$(echo $line | awk '{print $1}')
		de[$i]=$device
		echo $i $line
	else
		echo $line
	fi
	((i++))
done

echo -n "选择机子 "
read input
final=${de[$input]}

if [ ! -z $3 ]; then
	adb -s $final shell pm clear $1
else
	echo "0 clear app cache"
	echo "1 don't clear app cache"
	echo -n "选择数值："
	read chosen
	
	if [ $chosen -eq 0 ]; then
		echo "清理app cache"
		adb -s $final shell pm clear $1
	fi
fi
	
echo "开启App"
adb -s $final shell am start -n $1/$2
 
echo -n "手机上微信登陆成功后，回车继续"

read name

echo "拉取App的登陆信息"
if [[ $final =~ "emulator" ]]; then
	adb -s $final pull storage/sdcard0/Android/data/com.shangyou.bluff.hopoker/files/LoginInfo_test.data Debug
else
	adb -s $final pull storage/emulated/0/Android/data/com.shangyou.bluff.hopoker/files/LoginInfo_test.data Debug
fi

echo "关闭游戏"
adb -s $final shell am force-stop $1

if [ -z $3 ]; then
	read n
fi
