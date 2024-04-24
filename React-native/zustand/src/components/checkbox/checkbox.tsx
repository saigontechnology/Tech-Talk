import React, {FC, useRef} from 'react';
import {
  TouchableOpacity,
  Text,
  View,
  StyleSheet,
  Animated,
  ViewStyle,
  TextStyle,
} from 'react-native';

interface Props {
  text?: string;
  onPress: () => void;
  isChecked: boolean;
  containerStyle?: ViewStyle;
  textStyle?: TextStyle;
  checkboxStyle?: TextStyle;
}

export const Checkbox: FC<Props> = ({
  text,
  onPress,
  isChecked,
  containerStyle,
  textStyle,
  checkboxStyle,
}) => {
  const animatedWidth = useRef(new Animated.Value(0)).current;

  const startAnimation = () => {
    const toValue = isChecked ? 0 : 30;
    Animated.timing(animatedWidth, {
      toValue: toValue,
      duration: 500,
      useNativeDriver: false,
    }).start();
  };

  return (
    <View style={[styles.container, containerStyle]}>
      <TouchableOpacity
        onPress={() => {
          startAnimation();
          onPress();
        }}
        style={[
          styles.checkbox,
          isChecked && styles.checkboxSelected,
          checkboxStyle,
        ]}>
        <Animated.View style={{width: animatedWidth}}>
          <View style={[styles.checkbox, styles.icon]} />
        </Animated.View>
      </TouchableOpacity>
      <Text style={[styles.checkboxText, textStyle]}>{text}</Text>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  checkbox: {
    borderColor: 'green',
    borderWidth: 1,
    borderRadius: 5,
    height: 30,
    width: 30,
  },
  checkboxSelected: {
    backgroundColor: 'green',
  },
  checkboxText: {
    fontSize: 16,
    marginLeft: 10,
  },
  icon: {
    width: '100%',
    height: '100%',
    borderWidth: 0,
    backgroundColor: 'green',
  },
});
