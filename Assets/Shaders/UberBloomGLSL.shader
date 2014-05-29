
Shader "Custom\UberBloomGLSL" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Bloom ("Bloom (RGB)", 2D) = "black" {}
	}
	SubShader {  
		ZTest Off Cull Off ZWrite Off Blend Off
	  	Fog { Mode off }  
		// 0
		Pass {
			GLSLPROGRAM
			    uniform sampler2D _MainTex;	
         		uniform sampler2D _Bloom;	

				varying vec4 pos;
				varying vec2 uv;
				varying vec2 uv2;

				#ifdef VERTEX
					void main()
			        { 
						pos = gl_ModelViewProjectionMatrix * gl_Vertex;
			        	uv = gl_MultiTexCoord0;		
			        	
				        #if UNITY_UV_STARTS_AT_TOP
				        	uv2 = gl_MultiTexCoord0;				
				        	if (_MainTex_TexelSize.y < 0.0)
				        		uv.y = 1.0 - uv.y;
				        #endif
			
			        	gl_Position = pos;
					}
				#endif
				#ifdef FRAGMENT
					void main()
			        { 
			        	#if (UNITY_UV_STARTS_AT_TOP)
			        	
			      			vec4 color = texture2D(_MainTex, uv);
							gl_FragColor = color + texture2D(_Bloom, uv2);  		
			        	
						#else
						
							vec4 color = texture2D(_MainTex, uv);
							gl_FragColor = color + texture2D(_Bloom, uv);

						#endif
					}
				#endif
			ENDGLSL	 
		}

		// 1
		Pass {
			GLSLPROGRAM
				uniform sampler2D _MainTex;	
         		uniform vec4 _Parameter;
				uniform vec4 _MainTex_TexelSize;

         		varying vec4 pos;
         		varying vec2 uv20;
         		varying vec2 uv21;
         		varying vec2 uv22;
         		varying vec2 uv23;

         		#define ONE_MINUS_THRESHHOLD_TIMES_INTENSITY _Parameter.w
				#define THRESHHOLD _Parameter.z

				#ifdef VERTEX
					void main()
			        { 
						pos = gl_ModelViewProjectionMatrix * gl_Vertex;
				        uv20 = gl_MultiTexCoord0 + _MainTex_TexelSize.xy;				
						uv21 = gl_MultiTexCoord0 + _MainTex_TexelSize.xy * vec2(-0.5f,-0.5f);	
						uv22 = gl_MultiTexCoord0 + _MainTex_TexelSize.xy * vec2(0.5f,-0.5f);		
						uv23 = gl_MultiTexCoord0 + _MainTex_TexelSize.xy * vec2(-0.5f,0.5f);		
			        	gl_Position = pos;
					}
				#endif 
				#ifdef FRAGMENT
					void main() 
			        { 
			        	vec4 color = texture2D (_MainTex, uv20);
						color += texture2D (_MainTex, uv21);
						color += texture2D (_MainTex, uv22);
						color += texture2D (_MainTex, uv23);
						gl_FragColor = max(color/4 - THRESHHOLD, 0) * ONE_MINUS_THRESHHOLD_TIMES_INTENSITY;
					}
				#endif
			ENDGLSL	 
		}

		// 2
		Pass {
			ZTest Always
			Cull Off
			GLSLPROGRAM
				uniform sampler2D _MainTex;	
         		uniform sampler2D _Bloom;
         		uniform vec4 _Parameter;
         		uniform vec4 _MainTex_TexelSize;	

         		varying vec4 uv;
         		varying vec2 offs;

         		static const vec4 curve4[7] = { vec4(0.0205,0.0205,0.0205,0), vec4(0.0855,0.0855,0.0855,0), vec4(0.232,0.232,0.232,0),
				vec4(0.324,0.324,0.324,1), vec4(0.232,0.232,0.232,0), vec4(0.0855,0.0855,0.0855,0), vec4(0.0205,0.0205,0.0205,0) };

				//Vertical blur
				#ifdef VERTEX
					void main()
			        {	
						uv = vec4(gl_MultiTexCoord0.x, gl_MultiTexCoord0.y, 1, 1);
						offs = _MainTex_TexelSize.xy * vec2(0.0, 1.0) * _Parameter.x;
						 
			        	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
					}
				#endif
				#ifdef FRAGMENT
					void main()
			        { 
			        	vec2 uv = uv.xy; 
						vec2 netFilterWidth = offs;  
						vec2 coords = uv - netFilterWidth * 3.0;  
						
						vec4 color = 0;
			  			for( int l = 0; l < 7; l++ )  
			  			{   
							vec4 tap = texture2D(_MainTex, coords);
							color += tap * curve4[l];
							coords += netFilterWidth;
			  			}
			        	gl_FragColor = color;
					}
				#endif
			ENDGLSL	 
		}
			
		// 3	
		Pass {
			ZTest Always
			Cull Off
			GLSLPROGRAM
				uniform sampler2D _MainTex;	
         		uniform sampler2D _Bloom;	
         		uniform vec4 _Parameter;
         		uniform vec4 _MainTex_TexelSize;	

         		varying vec4 uv;
         		varying vec2 offs;

         		static const vec4 curve4[7] = { vec4(0.0205,0.0205,0.0205,0), vec4(0.0855,0.0855,0.0855,0), vec4(0.232,0.232,0.232,0),
				vec4(0.324,0.324,0.324,1), vec4(0.232,0.232,0.232,0), vec4(0.0855,0.0855,0.0855,0), vec4(0.0205,0.0205,0.0205,0) };

         		//Horizontal blur
				#ifdef VERTEX
					void main()
			        { 					
						uv = vec4(gl_MultiTexCoord0.x, gl_MultiTexCoord0.y, 1, 1);
						offs = _MainTex_TexelSize.xy * vec2(1.0, 0.0) * _Parameter.x;

			        	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
					}
				#endif
				#ifdef FRAGMENT
					void main()
			        { 
			        	vec2 uv = uv.xy; 
						vec2 netFilterWidth = offs;  
						vec2 coords = uv - netFilterWidth * 3.0;  
						
						vec4 color = 0;
			  			for( int l = 0; l < 7; l++ )  
			  			{   
							vec4 tap = texture2D(_MainTex, coords);
							color += tap * curve4[l];
							coords += netFilterWidth;
			  			}
			        	gl_FragColor = color;
					}
				#endif
			ENDGLSL	 
		}

		// alternate blur
		// 4
		Pass {
			ZTest Always
			Cull Off
			GLSLPROGRAM
				uniform sampler2D _MainTex;	
         		uniform sampler2D _Bloom;
         		uniform vec4 _Parameter;
         		uniform vec4 _MainTex_TexelSize;		

         		varying vec4 uv;
         		varying vec4 offs[3];

         		static const vec4 curve4[7] = { vec4(0.0205,0.0205,0.0205,0), vec4(0.0855,0.0855,0.0855,0), vec4(0.232,0.232,0.232,0),
				vec4(0.324,0.324,0.324,1), vec4(0.232,0.232,0.232,0), vec4(0.0855,0.0855,0.0855,0), vec4(0.0205,0.0205,0.0205,0) };

				#ifdef VERTEX
					void main()
			        { 
						uv = vec4(gl_MultiTexCoord0.x, gl_MultiTexCoord0.y,1,1);
						vec2 netFilterWidth = _MainTex_TexelSize.xy * vec2(0.0, 1.0) * _Parameter.x;
						vec4 coords = -netFilterWidth.xyxy * 3.0;
						offs[0] = gl_MultiTexCoord0.xyxy + coords * vec4(1.0,1.0,-1.0,-1.0);
						coords += netFilterWidth.xyxy;
						offs[1] = gl_MultiTexCoord0.xyxy + coords * vec4(1.0,1.0,-1.0,-1.0);
						coords += netFilterWidth.xyxy;
						offs[2] = gl_MultiTexCoord0.xyxy + coords * vec4(1.0,1.0,-1.0,-1.0);

			        	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
					}
				#endif
				#ifdef FRAGMENT
					void main()
			        { 			
			        	vec2 _uv = uv.xy;
						vec4 color = texture2D(_MainTex, _uv) * curve4[3];
						
			  			for( int l = 0; l < 3; l++ )  
			  			{   
							vec4 tapA = texture2D(_MainTex, (offs[l]).xy);
							vec4 tapB = texture2D(_MainTex, (offs[l]).zw); 
							color += (tapA + tapB) * curve4[l];
			  			}

						gl_FragColor = color;
					}
				#endif
			ENDGLSL	 
		}
			
		// 5
		Pass {
			ZTest Always
			Cull Off
			GLSLPROGRAM
				uniform sampler2D _MainTex;	
         		uniform sampler2D _Bloom;
         		uniform vec4 _Parameter;
         		uniform vec4 _MainTex_TexelSize;		

         		varying vec4 uv;
         		varying vec4 offs[3];

         		static const vec4 curve4[7] = { vec4(0.0205,0.0205,0.0205,0), vec4(0.0855,0.0855,0.0855,0), vec4(0.232,0.232,0.232,0),
				vec4(0.324,0.324,0.324,1), vec4(0.232,0.232,0.232,0), vec4(0.0855,0.0855,0.0855,0), vec4(0.0205,0.0205,0.0205,0) };

				#ifdef VERTEX
					void main()
			        { 
						uv = vec4(gl_MultiTexCoord0.x, gl_MultiTexCoord0.y,1,1);
						vec2 netFilterWidth = _MainTex_TexelSize.xy * vec2(1.0, 0.0) * _Parameter.x;
						vec4 coords = -netFilterWidth.xyxy * 3.0;
						offs[0] = gl_MultiTexCoord0.xyxy + coords * vec4(1.0,1.0,-1.0,-1.0);
						coords += netFilterWidth.xyxy;
						offs[1] = gl_MultiTexCoord0.xyxy + coords * vec4(1.0,1.0,-1.0,-1.0);
						coords += netFilterWidth.xyxy;
						offs[2] = gl_MultiTexCoord0.xyxy + coords * vec4(1.0,1.0,-1.0,-1.0);

			        	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
					}
				#endif
				#ifdef FRAGMENT
					void main()
			        { 			
			        	vec2 _uv = uv.xy;
						vec4 color = texture2D(_MainTex, _uv) * curve4[3];
						
			  			for( int l = 0; l < 3; l++ )  
			  			{   
							vec4 tapA = texture2D(_MainTex, (offs[l]).xy);
							vec4 tapB = texture2D(_MainTex, (offs[l]).zw); 
							color += (tapA + tapB) * curve4[l];
			  			}

						gl_FragColor = color;
					}
				#endif
			ENDGLSL	 
		} 
	}
}
