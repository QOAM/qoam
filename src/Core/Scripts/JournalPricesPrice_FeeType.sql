/ *   T o   p r e v e n t   a n y   p o t e n t i a l   d a t a   l o s s   i s s u e s ,   y o u   s h o u l d   r e v i e w   t h i s   s c r i p t   i n   d e t a i l   b e f o r e   r u n n i n g   i t   o u t s i d e   t h e   c o n t e x t   o f   t h e   d a t a b a s e   d e s i g n e r . * /  
 B E G I N   T R A N S A C T I O N  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 S E T   A R I T H A B O R T   O N  
 S E T   N U M E R I C _ R O U N D A B O R T   O F F  
 S E T   C O N C A T _ N U L L _ Y I E L D S _ N U L L   O N  
 S E T   A N S I _ N U L L S   O N  
 S E T   A N S I _ P A D D I N G   O N  
 S E T   A N S I _ W A R N I N G S   O N  
 C O M M I T  
 B E G I N   T R A N S A C T I O N  
 G O  
 A L T E R   T A B L E   d b o . J o u r n a l P r i c e s   A D D  
 	 P r i c e _ F e e T y p e   i n t   N O T   N U L L   C O N S T R A I N T   D F _ J o u r n a l P r i c e s _ P r i c e _ F e e T y p e   D E F A U L T   0  
 G O  
 A L T E R   T A B L E   d b o . J o u r n a l P r i c e s   S E T   ( L O C K _ E S C A L A T I O N   =   T A B L E )  
 G O  
 C O M M I T  
 s e l e c t   H a s _ P e r m s _ B y _ N a m e ( N ' d b o . J o u r n a l P r i c e s ' ,   ' O b j e c t ' ,   ' A L T E R ' )   a s   A L T _ P e r ,   H a s _ P e r m s _ B y _ N a m e ( N ' d b o . J o u r n a l P r i c e s ' ,   ' O b j e c t ' ,   ' V I E W   D E F I N I T I O N ' )   a s   V i e w _ d e f _ P e r ,   H a s _ P e r m s _ B y _ N a m e ( N ' d b o . J o u r n a l P r i c e s ' ,   ' O b j e c t ' ,   ' C O N T R O L ' )   a s   C o n t r _ P e r   