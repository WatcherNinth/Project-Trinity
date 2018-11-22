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

echo "清除缓存"
adb -s $final shell pm clear $1
echo "开启App"
adb -s $final shell am start -n $1/$2
read n