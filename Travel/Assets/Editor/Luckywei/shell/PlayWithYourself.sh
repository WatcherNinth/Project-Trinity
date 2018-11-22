sh Assets/Editor/Luckywei/get_wepoker_token.sh $1 $2 abc

adb shell pm clear $1

echo "游客登陆"

echo "开启App"
adb shell am start -n $1/$2
read n