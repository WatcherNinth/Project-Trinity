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

echo $i "所有机子"

echo -n "选择机子"
read input

if [ $i -eq $input ]; then
	for any in ${de[@]}
	do
		echo "device "$any
		echo "卸载游戏"
		adb -s $any uninstall $1
		echo "安装游戏"
		adb -s $any install apk/WePoker.apk
		echo "开启App"
		adb -s $any shell am start -n $1/$2
	done
	read n
else
	final=${de[$input]}

	echo "卸载游戏"
	echo $final $1
	adb -s $final uninstall $1
	echo "安装游戏"
	adb -s $final install apk/WePoker.apk
	echo "开启App"
	adb -s $final shell am start -n $1/$2
	read n
fi