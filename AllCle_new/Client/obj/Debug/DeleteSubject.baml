   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient  5   	       Client.DeleteSubject   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Client     5   	     � Title$  DeleteSubject��5   	   $	��400��6   $	��500��6,    � WindowStartupLocation$ CenterScreen=�68    � WindowStyle$
 None=�6]    � AllowsTransparency . ��5
      �� 5   
    �� Opacity$	 0.5Q�6    � 	Resources 5       �� 
TargetType   roundedButton �� Property �� Value	 �� 
TargetType
 ��Name �� CornerRadius ��Name �� Triggers P� Property P� Value �� 
TargetName��%  &	        �� 5   
   " ��6   .�5      ��  �@��TextAlignment$ Center=�68   �� 5       � �� 5      �� 5      "	 ��6&   .R�5      �� $
 button_border��6!   $ 10��68   #��z��6J   $��0q�6t   #��z��6�   ._�5      �� $ contentPresenter��6/   #��z��6I   #��z��6}   #��z��6�   #��z��6�   #��z��6  5       5      P� 5       �@J�IsMouseOver . . �5   "   �� $ button_border��6)    � ��
 �  ��6Z   5      5      5      5      5       .��5"      �� $ 40��6   
�������._�5#   
   �  � RowDefinitions 5$      �� 5%      $��2* �6    �� 5&      �� 5'      �� 5(      �� 5)      $
��0.8* �6    �� 5*      .+�5,      �� $��1��6   ._�5-      �� -   5        $��SubjectName_Tbk��5-      $$��'전공기초영어' 과목을��63   $��30(�6G   $��SemiBold"�6U   
�������$��Center=�6~   $��Center=�6�   5.      �� 5/      $��2��6   ._�50      �� $/��)시간표에서 삭제하시겠습니까?��6   $��30(�63   $��SemiBold"�6A   
�������$��Center=�6j   $��Center=�6�   51      � 52      $��4��6    � ColumnDefinitions 53      �� 54      �� 55      .+�57      �� -   5        $��
Cancel_btn��57      $��0��6+   $��취소��6;   
�������$��20(�6[   $��SemiBold"�6i   ���	#FF404040#��[  6�   $��	20 0 10 0q�6�   59      �� 5:      -   5        $��Confirm_btn��5:      $��1��6,   $��확인��6<   
�������$��20(�6\   $��SemiBold"�6j   ���	#FFF0C05A#��[  6�   $��	10 0 20 0q�6�   5<      5@      5F      5G      5H      