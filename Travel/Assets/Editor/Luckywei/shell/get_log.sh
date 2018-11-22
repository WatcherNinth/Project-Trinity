#！/bin/bash


function getLog()
{
	echo $1
	if [[ $1 =~ "emulator" ]]; then
		adb -s $1 pull storage/sdcard0/WePoker.log Debug
	else
		adb -s $1 pull storage/emulated/0/WePoker.log Debug
	fi
	mv Debug/WePoker.log Debug/$1.log
	echo "拷贝完成"
}

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

echo $i" 所有日志"

echo -n "选择机子 "
read input
final=${de[$input]}

if [ $input -eq $i ]; then
	for dev in ${de[@]}
	do
		getLog $dev
	done
else
	getLog $fianl
fi
	
read n